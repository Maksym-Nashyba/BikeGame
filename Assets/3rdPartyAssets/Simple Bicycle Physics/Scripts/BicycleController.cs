using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Please use using SBPScripts; directive to refer to or append the SBP library
namespace SBPScripts
{
    // Cycle Geometry Class - Holds Gameobjects pertaining to the specific bicycle
    [System.Serializable]
    public class CycleGeometry
    {
        public GameObject handles, lowerFork, fWheelVisual, RWheel, crank, lPedal, rPedal, fGear, rGear;
    }
    //Pedal Adjustments Class - Manipulates pedals and their positioning.  
    [System.Serializable]
    public class PedalAdjustments
    {
        public float crankRadius;
        public Vector3 lPedalOffset, rPedalOffset;
        public float pedalingSpeed;
    }
    // Wheel Friction Settings Class - Uses Physics Materials and Physics functions to control the 
    // static / dynamic slipping of the wheels 
    [System.Serializable]
    public class WheelFrictionSettings
    {
        public PhysicMaterial fPhysicMaterial, rPhysicMaterial;
        public Vector2 fFriction, rFriction;
    }
    // Way Point System Class - Replay Ghosting system
    [System.Serializable]
    public class WayPointSystem
    {
        public enum RecordingState { DoNothing, Record, Playback };
        public RecordingState recordingState = RecordingState.DoNothing;
        [Range(1, 10)]
        public int frameIncrement;
        [HideInInspector]
        public List<Vector3> bicyclePositionTransform;
        [HideInInspector]
        public List<Quaternion> bicycleRotationTransform;
        [HideInInspector]
        public List<Vector2Int> movementInstructionSet;
        [HideInInspector]
        public List<bool> sprintInstructionSet;
        [HideInInspector]
        public List<int> bHopInstructionSet;
    }
    [System.Serializable]
    public class AirTimeSettings
    {
        public bool freestyle;
        public float airTimeRotationSensitivity;
        [Range(0.5f, 10)]
        public float heightThreshold;
        public float groundSnapSensitivity;
    }
    public class BicycleController : MonoBehaviour
    {
        public CycleGeometry cycleGeometry;
        public GameObject fPhysicsWheel, rPhysicsWheel;
        public WheelFrictionSettings wheelFrictionSettings;
        // Curve of Power Exerted over Input time by the cyclist
        // This class sets the physics materials on to the
        // tires of the bicycle. F Friction pertains to the front tire friction and R Friction to
        // the rear. They are of the Vector2 type. X field edits the static friction
        // information and Y edits the dynamic friction. Please keep the values over 0.5.
        // For more information, please read the commented scripts.
        public AnimationCurve accelerationCurve;
        [Tooltip("Steer Angle over Speed")]
        public AnimationCurve steerAngle;
        public float axisAngle;
        // Defines the leaning curve of the bicycle
        public AnimationCurve leanCurve;
        // The slider refers to the ratio of Relaxed mode to Top Speed. 
        // Torque is a physics based function which acts as the actual wheel driving force.
        public float torque, topSpeed;
        [Range(0.1f, 0.9f)]
        [Tooltip("Ratio of Relaxed mode to Top Speed")]
        public float relaxedSpeed;
        public float reversingSpeed;
        public Vector3 centerOfMassOffset;
        [HideInInspector]
        public bool isReversing, isAirborne, stuntMode;
        // Controls Cycle sway from left to right.
        // The degree of cycle waddling side to side upon pedaling.
        // Higher values correspond to higher waddling. This property also affects
        // character IK. 

        [Range(0, 8)]
        public float oscillationAmount;
        // Following the natural movement of a cyclist, the
        // oscillation of the cycle from side to side also affects the steering to a certain
        // extent. This value refers to the counter steer upon cycle oscillation. Higher
        // values correspond to a higher percentage of the oscillation being transferred
        // to the steering handles. 

        [Range(0, 1)]
        public float oscillationAffectSteerRatio;
        float oscillationSteerEffect;
        [HideInInspector]
        public float cycleOscillation;
        [HideInInspector]
        public Rigidbody rb, fWheelRb, rWheelRb;
        float turnAngle;
        float xQuat, zQuat;
        [HideInInspector]
        public float crankSpeed, crankCurrentQuat, crankLastQuat, restingCrank;
        public PedalAdjustments pedalAdjustments;
        [HideInInspector]
        public float turnLeanAmount;
        RaycastHit hit;
        //[HideInInspector]
        public float SteerAxis, LeanAxis, AccelerationAxis, rawAcceleration;
        bool isRaw, sprint;
        [HideInInspector]
        public int bunnyHopInputState;
        [HideInInspector]
        public float currentTopSpeed, pickUpSpeed;
        Quaternion initialLowerForkLocalRotaion, initialHandlesRotation;
        ConfigurableJoint fPhysicsWheelConfigJoint, rPhysicsWheelConfigJoint;
        // Ground Conformity refers to vehicles that do not need a gyroscopic force to keep them upright.
        // For non-gyroscopic wheel systems like the tricycle,
        // enabling ground conformity ensures that the tricycle is not always upright and
        // follows the curvature of the terrain. 
        public bool groundConformity;
        RaycastHit hitGround;
        Vector3 theRay;
        float groundZ;
        JointDrive fDrive, rYDrive, rZDrive;
        // Attempts to Reduce/eliminate bouncing of the bicycle after a fall impact 
        public bool inelasticCollision;
        [HideInInspector]
        public Vector3 lastVelocity, deceleration, lastDeceleration;
        int impactFrames;
        bool isBunnyHopping;
        [HideInInspector]
        public float BunnyHopAmount;
        // The upward force the rider can bunny hop with. 
        public float BunnyHopStrength;
        public WayPointSystem WayPointSystem;
        public AirTimeSettings AirTimeSettings;
        private IBikeInputProvider _inputProvider;

        private void Awake()
        {
            _inputProvider = GetComponent<IBikeInputProvider>();
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.maxAngularVelocity = Mathf.Infinity;

            fWheelRb = fPhysicsWheel.GetComponent<Rigidbody>();
            fWheelRb.maxAngularVelocity = Mathf.Infinity;

            rWheelRb = rPhysicsWheel.GetComponent<Rigidbody>();
            rWheelRb.maxAngularVelocity = Mathf.Infinity;

            currentTopSpeed = topSpeed;

            initialHandlesRotation = cycleGeometry.handles.transform.localRotation;
            initialLowerForkLocalRotaion = cycleGeometry.lowerFork.transform.localRotation;

            fPhysicsWheelConfigJoint = fPhysicsWheel.GetComponent<ConfigurableJoint>();
            rPhysicsWheelConfigJoint = rPhysicsWheel.GetComponent<ConfigurableJoint>();

            //Recording is set to 0 to remove the recording previous data if not set to playback
            if (WayPointSystem.recordingState == WayPointSystem.RecordingState.Record || WayPointSystem.recordingState == WayPointSystem.RecordingState.DoNothing)
            {
                WayPointSystem.bicyclePositionTransform.Clear();
                WayPointSystem.bicycleRotationTransform.Clear();
                WayPointSystem.movementInstructionSet.Clear();
                WayPointSystem.sprintInstructionSet.Clear();
                WayPointSystem.bHopInstructionSet.Clear();
            }
        }

        private void FixedUpdate()
        {

            //Physics based Steering Control.
            fPhysicsWheel.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + SteerAxis * steerAngle.Evaluate(rb.velocity.magnitude) + oscillationSteerEffect, 0);
            fPhysicsWheelConfigJoint.axis = new Vector3(1, 0, 0);

            //Power Control. Wheel Torque + Acceleration curves

            //cache rb velocity
            float currentSpeed = rb.velocity.magnitude;

            if (!sprint)
                currentTopSpeed = Mathf.Lerp(currentTopSpeed, topSpeed * relaxedSpeed, Time.deltaTime);
            else
                currentTopSpeed = Mathf.Lerp(currentTopSpeed, topSpeed, Time.deltaTime);

            if (currentSpeed < currentTopSpeed && rawAcceleration > 0)
                rWheelRb.AddTorque(transform.right * torque * AccelerationAxis);

            if (currentSpeed < currentTopSpeed && rawAcceleration > 0 && !isAirborne && !isBunnyHopping)
                rb.AddForce(transform.forward * accelerationCurve.Evaluate(AccelerationAxis));

            if (currentSpeed < reversingSpeed && rawAcceleration < 0 && !isAirborne && !isBunnyHopping)
                rb.AddForce(-transform.forward * accelerationCurve.Evaluate(AccelerationAxis) * 0.5f);

            if (transform.InverseTransformDirection(rb.velocity).z < 0)
                isReversing = true;
            else
                isReversing = false;

            if (rawAcceleration < 0 && isReversing == false && !isAirborne && !isBunnyHopping)
                rb.AddForce(-transform.forward * accelerationCurve.Evaluate(AccelerationAxis) * 2);

            // Center of Mass handling
            if (stuntMode)
                rb.centerOfMass = GetComponent<BoxCollider>().center;
            else
                rb.centerOfMass = Vector3.zero + centerOfMassOffset;

            //Handles
            cycleGeometry.handles.transform.localRotation = Quaternion.Euler(0, SteerAxis * steerAngle.Evaluate(currentSpeed) + oscillationSteerEffect * 5, 0) * initialHandlesRotation;

            //LowerFork
            cycleGeometry.lowerFork.transform.localRotation = Quaternion.Euler(0, SteerAxis * steerAngle.Evaluate(currentSpeed) + oscillationSteerEffect * 5, SteerAxis * -axisAngle) * initialLowerForkLocalRotaion;

            //FWheelVisual
            xQuat = Mathf.Sin(Mathf.Deg2Rad * (transform.rotation.eulerAngles.y));
            zQuat = Mathf.Cos(Mathf.Deg2Rad * (transform.rotation.eulerAngles.y));
            cycleGeometry.fWheelVisual.transform.rotation = Quaternion.Euler(xQuat * (SteerAxis * -axisAngle), SteerAxis * steerAngle.Evaluate(currentSpeed) + oscillationSteerEffect * 5, zQuat * (SteerAxis * -axisAngle));
            cycleGeometry.fWheelVisual.transform.GetChild(0).transform.localRotation = cycleGeometry.RWheel.transform.rotation;

            //Crank
            crankCurrentQuat = cycleGeometry.RWheel.transform.rotation.eulerAngles.x;
            if (AccelerationAxis > 0 && !isAirborne && !isBunnyHopping)
            {
                crankSpeed += Mathf.Sqrt(AccelerationAxis * Mathf.Abs(Mathf.DeltaAngle(crankCurrentQuat, crankLastQuat) * pedalAdjustments.pedalingSpeed));
                crankSpeed %= 360;
            }
            else if (Mathf.Floor(crankSpeed) > restingCrank)
                crankSpeed += -6;
            else if (Mathf.Floor(crankSpeed) < restingCrank)
                crankSpeed = Mathf.Lerp(crankSpeed, restingCrank, Time.deltaTime * 5);

            crankLastQuat = crankCurrentQuat;
            cycleGeometry.crank.transform.localRotation = Quaternion.Euler(crankSpeed, 0, 0);

            //Pedals
            cycleGeometry.lPedal.transform.localPosition = pedalAdjustments.lPedalOffset + new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * (crankSpeed + 180)) * pedalAdjustments.crankRadius, Mathf.Sin(Mathf.Deg2Rad * (crankSpeed + 180)) * pedalAdjustments.crankRadius);
            cycleGeometry.rPedal.transform.localPosition = pedalAdjustments.rPedalOffset + new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * (crankSpeed)) * pedalAdjustments.crankRadius, Mathf.Sin(Mathf.Deg2Rad * (crankSpeed)) * pedalAdjustments.crankRadius);

            //FGear
            if (cycleGeometry.fGear != null)
                cycleGeometry.fGear.transform.rotation = cycleGeometry.crank.transform.rotation;
            //RGear
            if (cycleGeometry.rGear != null)
                cycleGeometry.rGear.transform.rotation = rPhysicsWheel.transform.rotation;

            //CycleOscillation
            if ((sprint && currentSpeed > 5 && isReversing == false) || isAirborne || isBunnyHopping)
                pickUpSpeed += Time.deltaTime * 2;
            else
                pickUpSpeed -= Time.deltaTime * 2;

            pickUpSpeed = Mathf.Clamp(pickUpSpeed, 0.1f, 1);

            cycleOscillation = -Mathf.Sin(Mathf.Deg2Rad * (crankSpeed + 90)) * (oscillationAmount * (Mathf.Clamp(currentTopSpeed / currentSpeed, 1f, 1.5f))) * pickUpSpeed;
            turnLeanAmount = -leanCurve.Evaluate(LeanAxis) * Mathf.Clamp(currentSpeed * 0.1f, 0, 1);
            oscillationSteerEffect = cycleOscillation * Mathf.Clamp01(AccelerationAxis) * (oscillationAffectSteerRatio * (Mathf.Clamp(topSpeed / currentSpeed, 1f, 1.5f)));

            //FrictionSettings
            wheelFrictionSettings.fPhysicMaterial.staticFriction = wheelFrictionSettings.fFriction.x;
            wheelFrictionSettings.fPhysicMaterial.dynamicFriction = wheelFrictionSettings.fFriction.y;
            wheelFrictionSettings.rPhysicMaterial.staticFriction = wheelFrictionSettings.rFriction.x;
            wheelFrictionSettings.rPhysicMaterial.dynamicFriction = wheelFrictionSettings.rFriction.y;

            if (Physics.Raycast(fPhysicsWheel.transform.position, Vector3.down, out hit, Mathf.Infinity))
                if (hit.distance < 0.5f)
                {
                    Vector3 velf = fPhysicsWheel.transform.InverseTransformDirection(fWheelRb.velocity);
                    velf.x *= Mathf.Clamp01(1 / (wheelFrictionSettings.fFriction.x + wheelFrictionSettings.fFriction.y));
                    fWheelRb.velocity = fPhysicsWheel.transform.TransformDirection(velf);
                }
            if (Physics.Raycast(rPhysicsWheel.transform.position, Vector3.down, out hit, Mathf.Infinity))
                if (hit.distance < 0.5f)
                {
                    Vector3 velr = rPhysicsWheel.transform.InverseTransformDirection(rWheelRb.velocity);
                    velr.x *= Mathf.Clamp01(1 / (wheelFrictionSettings.rFriction.x + wheelFrictionSettings.rFriction.y));
                    rWheelRb.velocity = rPhysicsWheel.transform.TransformDirection(velr);
                }

            //Impact sensing
            deceleration = (fWheelRb.velocity - lastVelocity) / Time.fixedDeltaTime;
            lastVelocity = fWheelRb.velocity;
            impactFrames--;
            impactFrames = Mathf.Clamp(impactFrames, 0, 15);
            if (deceleration.y > 200 && lastDeceleration.y < -1)
                impactFrames = 30;

            lastDeceleration = deceleration;

            if (impactFrames > 0 && inelasticCollision)
            {
                fWheelRb.velocity = new Vector3(fWheelRb.velocity.x, -Mathf.Abs(fWheelRb.velocity.y), fWheelRb.velocity.z);
                rWheelRb.velocity = new Vector3(rWheelRb.velocity.x, -Mathf.Abs(rWheelRb.velocity.y), rWheelRb.velocity.z);
            }

            //AirControl
            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.distance > 1.5f || impactFrames > 0)
                {
                    isAirborne = true;
                    restingCrank = 100;
                }
                else if (isBunnyHopping)
                {
                    restingCrank = 100;
                }
                else
                {
                    isAirborne = false;
                    restingCrank = 10;
                }
                // For stunts
                // 5f is the snap to ground distance
                if (hit.distance > AirTimeSettings.heightThreshold && AirTimeSettings.freestyle)
                {
                    stuntMode = true;
                    // Stunt + flips controls (Not available for Waypoint system as of yet)
                    // You may use Numpad Inputs as well.
                    rb.AddTorque(Vector3.up * SteerAxis * 4 * AirTimeSettings.airTimeRotationSensitivity, ForceMode.Impulse);
                    rb.AddTorque(transform.right * rawAcceleration * -3 * AirTimeSettings.airTimeRotationSensitivity, ForceMode.Impulse);
                }
                else
                    stuntMode = false;
            }

            // Setting the Main Rotational movements of the bicycle
            if (AirTimeSettings.freestyle)
            {
                if (!stuntMode && isAirborne)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.rotation.eulerAngles.y, turnLeanAmount + cycleOscillation + GroundConformity(groundConformity)), Time.deltaTime * AirTimeSettings.groundSnapSensitivity);
                else if (!stuntMode && !isAirborne)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, turnLeanAmount + cycleOscillation + GroundConformity(groundConformity)), Time.deltaTime * 10 * AirTimeSettings.groundSnapSensitivity);
            }
            else
            {
                //Pre-version 1.5
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, turnLeanAmount + cycleOscillation + GroundConformity(groundConformity));
            }


        }
        private void Update()
        {
            ApplyCustomInput();

            //GetKeyUp/Down requires an Update Cycle
            //BunnyHopping
            if (bunnyHopInputState == 1)
            {
                isBunnyHopping = true;
                BunnyHopAmount += Time.deltaTime * 8f;
            }
            if (bunnyHopInputState == -1)
                StartCoroutine(DelayBunnyHop());

            if (bunnyHopInputState == -1 && !isAirborne)
                rb.AddForce(transform.up * BunnyHopAmount * BunnyHopStrength, ForceMode.VelocityChange);
            else
                BunnyHopAmount = Mathf.Lerp(BunnyHopAmount, 0, Time.deltaTime * 8f);

            BunnyHopAmount = Mathf.Clamp01(BunnyHopAmount);

        }
        private float GroundConformity(bool toggle)
        {
            if (toggle)
            {
                groundZ = transform.rotation.eulerAngles.z;
            }
            return groundZ;

        }

        private void ApplyCustomInput()
        {
            if (WayPointSystem.recordingState == WayPointSystem.RecordingState.DoNothing || WayPointSystem.recordingState == WayPointSystem.RecordingState.Record)
            {
                InputValues inputs = _inputProvider.GetCurrentInput(transform);
                
                CustomInput(inputs.Steer, ref SteerAxis, 5, 5, true);
                CustomInput(inputs.Acceleration, ref AccelerationAxis, 1, 1, true);
                CustomInput(inputs.Steer, ref LeanAxis, 1, 1, false);
                CustomInput(inputs.Acceleration, ref rawAcceleration, 1, 1, true);

                sprint = Input.GetKey(KeyCode.LeftShift);

                if (inputs.BrakesHit && Mathf.Abs(inputs.Steer) > 0f && !isAirborne)
                {
                    float sign = inputs.Steer / Mathf.Abs(inputs.Steer);
                    Vector3 right = transform.right;
                    fWheelRb.AddForce(right * 30f * sign); 
                    rWheelRb.AddForce(right * 80f * -sign);
                    fWheelRb.velocity *= (1f - Time.deltaTime) * 0.85f;
                }

                //Stateful Input - bunny hopping
                if (Input.GetKey(KeyCode.Space))
                    bunnyHopInputState = 1;
                else if (Input.GetKeyUp(KeyCode.Space))
                    bunnyHopInputState = -1;
                else
                    bunnyHopInputState = 0;

                //Record
                if (WayPointSystem.recordingState == WayPointSystem.RecordingState.Record)
                {
                    if (Time.frameCount % WayPointSystem.frameIncrement == 0)
                    {
                        WayPointSystem.bicyclePositionTransform.Add(new Vector3(Mathf.Round(transform.position.x * 100f) * 0.01f, Mathf.Round(transform.position.y * 100f) * 0.01f, Mathf.Round(transform.position.z * 100f) * 0.01f));
                        WayPointSystem.bicycleRotationTransform.Add(transform.rotation);
                        WayPointSystem.movementInstructionSet.Add(new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical")));
                        WayPointSystem.sprintInstructionSet.Add(sprint);
                        WayPointSystem.bHopInstructionSet.Add(bunnyHopInputState);
                    }
                }
            }
            else
            {
                if (WayPointSystem.recordingState == WayPointSystem.RecordingState.Playback)
                {
                    if (WayPointSystem.movementInstructionSet.Count - 1 > Time.frameCount / WayPointSystem.frameIncrement)
                    {
                        transform.position = Vector3.Lerp(transform.position, WayPointSystem.bicyclePositionTransform[Time.frameCount / WayPointSystem.frameIncrement], Time.deltaTime * WayPointSystem.frameIncrement);
                        transform.rotation = Quaternion.Lerp(transform.rotation, WayPointSystem.bicycleRotationTransform[Time.frameCount / WayPointSystem.frameIncrement], Time.deltaTime * WayPointSystem.frameIncrement);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].x, ref SteerAxis, 5, 5, false);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].y, ref AccelerationAxis, 1, 1, false);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].x, ref LeanAxis, 1, 1, false);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].y, ref rawAcceleration, 1, 1, true);
                        sprint = WayPointSystem.sprintInstructionSet[Time.frameCount / WayPointSystem.frameIncrement];
                        bunnyHopInputState = WayPointSystem.bHopInstructionSet[Time.frameCount / WayPointSystem.frameIncrement];
                    }
                }
            }
        }

        //Input Manager Controls
        private void CustomInput(float raw, ref float axis, float sensitivity, float gravity, bool isRaw)
        { 
            float deltaTime = Time.unscaledDeltaTime;

            if (isRaw)
                axis = raw;
            else
            {
                if (raw >=Mathf.Epsilon)
                    axis = Mathf.Clamp(axis + raw * sensitivity * deltaTime, -1f, 1f);
                else
                    axis = Mathf.Clamp01(Mathf.Abs(axis) - gravity * deltaTime) * Mathf.Sign(axis);
            }
        }

        private void WayPointInput(float instruction, ref float axis, float sensitivity, float gravity, bool isRaw)
        {
            var time = Time.unscaledDeltaTime;

            if (isRaw)
                axis = instruction;
            else
            {
                if (instruction != 0)
                    axis = Mathf.Clamp(axis + instruction * sensitivity * time, -1f, 1f);
                else
                    axis = Mathf.Clamp01(Mathf.Abs(axis) - gravity * time) * Mathf.Sign(axis);
            }
        }

        private IEnumerator DelayBunnyHop()
        {
            yield return new WaitForSeconds(0.5f);
            isBunnyHopping = false;
            yield return null;
        }
    }
}