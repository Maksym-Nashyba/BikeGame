using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Pausing;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SBPScripts
{
    #region LocalClasses
    
    [Serializable]
    public class CycleGeometry
    {
        public GameObject handles, lowerFork, fWheelVisual, RWheel, crank, lPedal, rPedal, fGear, rGear;
    }
    
    //Pedal Adjustments Class - Manipulates pedals and their positioning.  
    [Serializable]
    public class PedalAdjustments
    {
        public float crankRadius;
        public Vector3 lPedalOffset, rPedalOffset;
        public float pedalingSpeed;
    }
    
    // Wheel Friction Settings Class - Uses Physics Materials and Physics functions to control the 
    // static / dynamic slipping of the wheels 
    [Serializable]
    public class WheelFrictionSettings
    {
        public PhysicMaterial fPhysicMaterial, rPhysicMaterial;
        public Vector2 fFriction, rFriction;
    }
    // Way Point System Class - Replay Ghosting system
    [Serializable]
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
    [Serializable]
    public class AirTimeSettings
    {
        public bool freestyle;
        public float airTimeRotationSensitivity;
        [Range(0.5f, 10)]
        public float heightThreshold;
        public float groundSnapSensitivity;
    }

    #endregion
    
    public class BicycleController : MonoBehaviour, IPausable
    {
        #region FUCKTON of fields

        [FormerlySerializedAs("cycleGeometry")] public CycleGeometry bicycleParts;
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
        public AnimationCurve velocityToSteerAngleCurve;
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
        public bool isMovingBackwards, isAirborne, stuntMode;
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
        [FormerlySerializedAs("rigidbody")] [FormerlySerializedAs("rb")] [HideInInspector]
        public Rigidbody Rigidbody;

        [HideInInspector]
        public Rigidbody fWheelRb, rWheelRb;

        float turnAngle;
        float xQuat, zQuat;

        [HideInInspector]
        public float crankSpeed;

        [FormerlySerializedAs("currentGear")] [FormerlySerializedAs("crankCurrentQuat")] [HideInInspector]
        public float CurrentRearWheelRevolution;

        [FormerlySerializedAs("crankLastQuat")] [HideInInspector]
        public float lastFraRearWheelRevolution;

        [HideInInspector]
        public float restingCrank;

        public PedalAdjustments pedalAdjustments;
        [HideInInspector]
        public float turnLeanAmount;
        RaycastHit hit;
        //[HideInInspector]
        [FormerlySerializedAs("SteerMultiplier")] public float SteerInput;
        [FormerlySerializedAs("LeanAxis")] public float LeanInput;
        public float AccelerationInput, rawAccelerationInput;
        bool isRaw, _isSprinting;
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
        private Transform _transform;
        private bool _isPaused;
        
        #endregion

        private void Awake()
        {
            _transform = transform;
            _inputProvider = GetComponent<IBikeInputProvider>();//TODO resolve dependencies through service locator
            _transform.rotation = Quaternion.Euler(0, _transform.rotation.eulerAngles.y, 0);
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Rigidbody.maxAngularVelocity = Mathf.Infinity;

            fWheelRb = fPhysicsWheel.GetComponent<Rigidbody>();
            fWheelRb.maxAngularVelocity = Mathf.Infinity;

            rWheelRb = rPhysicsWheel.GetComponent<Rigidbody>();
            rWheelRb.maxAngularVelocity = Mathf.Infinity;

            currentTopSpeed = topSpeed;

            initialHandlesRotation = bicycleParts.handles.transform.localRotation;
            initialLowerForkLocalRotaion = bicycleParts.lowerFork.transform.localRotation;

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
        
        private void Update()
        {
            if (_isPaused) return;
            
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
                Rigidbody.AddForce(transform.up * (BunnyHopAmount * BunnyHopStrength), ForceMode.VelocityChange);
            else
                BunnyHopAmount = Mathf.Lerp(BunnyHopAmount, 0, Time.deltaTime * 8f);

            BunnyHopAmount = Mathf.Clamp01(BunnyHopAmount);

        }
        
        private void FixedUpdate()
        {
            if (_isPaused) return;

            float currentSpeed = Rigidbody.velocity.magnitude;
            
            RotateForwardPhysicsWheel(velocityToSteerAngleCurve.Evaluate(currentSpeed));
            fPhysicsWheelConfigJoint.axis = new Vector3(1, 0, 0);

            currentTopSpeed = CalculateCurrentTopSpeed(_isSprinting);
            ApplyAccelerationForces(currentSpeed, !isAirborne && !isBunnyHopping);
            PositionCenterOfForce(stuntMode);
            RotateGearStars();
            UpdateVisuals(currentSpeed);
            RotateForwardWheel(currentSpeed);
            CalculateSideToSideWobbling(currentSpeed);
            ApplyFriction(currentSpeed);
            DetectLanding();
            ApplyAirControlForces();
            if (DetectSidewaysSliding(currentSpeed))
            {
                AlignToMovementDirection();
            }
        }

        private void AlignToMovementDirection()
        {
            Vector3 direction = Rigidbody.velocity.normalized;
            //Rigidbody.MoveRotation(Quaternion.Euler(direction));
            Debug.Log("FIRED");
        }

        private bool DetectSidewaysSliding(float currentSpeed)
        {
            if(currentSpeed < 2 || isAirborne) return false;
            Vector3 bikeForward = _transform.InverseTransformDirection(_transform.forward);
            bikeForward.y = 0;
            Vector3 movementDirection = _transform.InverseTransformDirection(Rigidbody.velocity.normalized);
            movementDirection.y = 0;
            float angle = Vector3.Angle(bikeForward, movementDirection)/180f;
            return Mathf.Sin(angle * Mathf.PI) > 0.40f;
        }

        private void ApplyAirControlForces()
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0),
                    Vector3.down, out hit, Mathf.Infinity))
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
                    Rigidbody.AddTorque(Vector3.up * (SteerInput * 4 * AirTimeSettings.airTimeRotationSensitivity),
                        ForceMode.Impulse);
                    Rigidbody.AddTorque(transform.right * (rawAccelerationInput * -3 * AirTimeSettings.airTimeRotationSensitivity),
                        ForceMode.Impulse);
                }
                else
                    stuntMode = false;
            }

            // Setting the Main Rotational movements of the bicycle
            if (AirTimeSettings.freestyle)
            {
                if (!stuntMode && isAirborne)
                    _transform.rotation = Quaternion.Lerp(_transform.rotation,
                        Quaternion.Euler(0,
                            _transform.rotation.eulerAngles.y,
                            turnLeanAmount + cycleOscillation + GroundConformity(groundConformity)),
                        Time.deltaTime * AirTimeSettings.groundSnapSensitivity);

                else if (!stuntMode && !isAirborne)
                    _transform.rotation = Quaternion.Lerp(_transform.rotation,
                        Quaternion.Euler(_transform.rotation.eulerAngles.x,
                            _transform.rotation.eulerAngles.y,
                            turnLeanAmount + cycleOscillation + GroundConformity(groundConformity)),
                        Time.deltaTime * 10 * AirTimeSettings.groundSnapSensitivity);
            }
            else
            {
                _transform.rotation = Quaternion.Euler(_transform.rotation.eulerAngles.x,
                    _transform.rotation.eulerAngles.y,
                    turnLeanAmount + cycleOscillation + GroundConformity(groundConformity));
            }
        }

        private void DetectLanding()
        {
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
        }

        private void CalculateSideToSideWobbling(float currentSpeed)
        {
            bool isSprintingForward = _isSprinting && currentSpeed > 5 && !isMovingBackwards;
            if (isSprintingForward || isAirborne || isBunnyHopping)
                pickUpSpeed += Time.deltaTime * 2;
            else
                pickUpSpeed -= Time.deltaTime * 2;

            pickUpSpeed = Mathf.Clamp(pickUpSpeed, 0.1f, 1);

            cycleOscillation = -Mathf.Sin(Mathf.Deg2Rad * (crankSpeed + 90)) *
                               (oscillationAmount * Mathf.Clamp(currentTopSpeed / currentSpeed, 1f, 1.5f)) * pickUpSpeed;
            turnLeanAmount = -leanCurve.Evaluate(LeanInput) * Mathf.Clamp(currentSpeed * 0.1f, 0, 1);
            oscillationSteerEffect = cycleOscillation * Mathf.Clamp01(AccelerationInput) *
                                     (oscillationAffectSteerRatio * Mathf.Clamp(topSpeed / currentSpeed, 1f, 1.5f));
        }

        private float CalculateCurrentTopSpeed(bool isSprinting)
        {
            float targetTopSpeed = isSprinting ? topSpeed : topSpeed * relaxedSpeed;
            return Mathf.Lerp(currentTopSpeed, targetTopSpeed, Time.deltaTime);
        }
        
        private void RotateForwardPhysicsWheel(float angle)
        {
            fPhysicsWheel.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
                _transform.rotation.eulerAngles.y + angle * SteerInput + oscillationSteerEffect, 0);
        }

        private void ApplyAccelerationForces(float currentSpeed, bool isGrounded)
        {
            if (currentSpeed < currentTopSpeed && rawAccelerationInput > 0)
                rWheelRb.AddTorque(_transform.right * (torque * AccelerationInput));

            if(!isGrounded) return;
            
            if (currentSpeed < currentTopSpeed && rawAccelerationInput > 0) //accelerate forward if input forward
                Rigidbody.AddForce(_transform.forward * accelerationCurve.Evaluate(AccelerationInput));

            if (currentSpeed < reversingSpeed && rawAccelerationInput < 0) //accelerate backwards if input backwards
                Rigidbody.AddForce(-_transform.forward * (accelerationCurve.Evaluate(AccelerationInput) * 0.5f));

            isMovingBackwards = transform.InverseTransformDirection(Rigidbody.velocity).z < 0;

            if (rawAccelerationInput < 0 && !isMovingBackwards) //slow down faster
                Rigidbody.AddForce(-_transform.forward * (accelerationCurve.Evaluate(AccelerationInput) * 2));
        }

        private void PositionCenterOfForce(bool stuntModeIsOn)
        {
            Rigidbody.centerOfMass = stuntModeIsOn ? GetComponent<BoxCollider>().center : Vector3.zero + centerOfMassOffset;
        }

        private void RotateHandles(float currentSpeed)
        {
            bicycleParts.handles.transform.localRotation = Quaternion.Euler(0, SteerInput * velocityToSteerAngleCurve.Evaluate(currentSpeed) + oscillationSteerEffect * 5, 0) * initialHandlesRotation;
        }

        private void RotateLowerFork(float currentSpeed)
        {
            bicycleParts.lowerFork.transform.localRotation = Quaternion.Euler(0, SteerInput * velocityToSteerAngleCurve.Evaluate(currentSpeed) + oscillationSteerEffect * 5, SteerInput * -axisAngle) * initialLowerForkLocalRotaion;
        }

        private void RotateForwardWheel(float currentSpeed)
        {
            Quaternion rotation = _transform.rotation;
            xQuat = Mathf.Sin(Mathf.Deg2Rad * rotation.eulerAngles.y);
            zQuat = Mathf.Cos(Mathf.Deg2Rad * rotation.eulerAngles.y);
            bicycleParts.fWheelVisual.transform.rotation = Quaternion.Euler(xQuat * (SteerInput * -axisAngle), SteerInput * velocityToSteerAngleCurve.Evaluate(currentSpeed) + oscillationSteerEffect * 5, zQuat * (SteerInput * -axisAngle));
            bicycleParts.fWheelVisual.transform.GetChild(0).transform.localRotation = bicycleParts.RWheel.transform.rotation;
            _transform.rotation = rotation;
        }
        
        private void UpdateVisuals(float currentSpeed)
        {
            RotateHandles(currentSpeed);
            RotateLowerFork(currentSpeed);
            RotateGears();
            UpdatePedalsPosition();
        }

        private void RotateGears()
        {
            if (bicycleParts.fGear is not null)
                bicycleParts.fGear.transform.rotation = bicycleParts.crank.transform.rotation;
            if (bicycleParts.rGear is not null)
                bicycleParts.rGear.transform.rotation = rPhysicsWheel.transform.rotation;
        }

        private void ApplyFriction(float currentSpeed)
        {
            float fFriction = 0.64f + currentSpeed / 15f * 0.15f;
            wheelFrictionSettings.fFriction = new Vector2(fFriction, fFriction);
            
            float rFriction = 0.69f + currentSpeed / 15f * 0.19f;
            wheelFrictionSettings.rFriction = new Vector2(rFriction, rFriction);
            
            wheelFrictionSettings.fPhysicMaterial.staticFriction = wheelFrictionSettings.fFriction.x;
            wheelFrictionSettings.fPhysicMaterial.dynamicFriction = wheelFrictionSettings.fFriction.y;
            wheelFrictionSettings.rPhysicMaterial.staticFriction = wheelFrictionSettings.rFriction.x;
            wheelFrictionSettings.rPhysicMaterial.dynamicFriction = wheelFrictionSettings.rFriction.y;

            if (Physics.Raycast(fPhysicsWheel.transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.distance < 0.5f) 
                    UpdateWheelVelocity(fPhysicsWheel, fWheelRb, wheelFrictionSettings.fFriction);
            }
            
            if (Physics.Raycast(rPhysicsWheel.transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.distance < 0.5f) 
                    UpdateWheelVelocity(rPhysicsWheel, rWheelRb, wheelFrictionSettings.rFriction);
            }
            
            void UpdateWheelVelocity(GameObject physicsWheel, Rigidbody wheelRigidbody, Vector2 wheelFriction)
            {
                Vector3 calculatedVelocity = physicsWheel.transform.InverseTransformDirection(wheelRigidbody.velocity);
                calculatedVelocity.x *= Mathf.Clamp01(1 / (wheelFriction.x + wheelFriction.y));
                wheelRigidbody.velocity = physicsWheel.transform.TransformDirection(calculatedVelocity);
            }
        }

        private void UpdatePedalsPosition()
        {
            bicycleParts.lPedal.transform.localPosition = pedalAdjustments.lPedalOffset + new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * (crankSpeed + 180)) * pedalAdjustments.crankRadius, Mathf.Sin(Mathf.Deg2Rad * (crankSpeed + 180)) * pedalAdjustments.crankRadius);
            bicycleParts.rPedal.transform.localPosition = pedalAdjustments.rPedalOffset + new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * crankSpeed) * pedalAdjustments.crankRadius, Mathf.Sin(Mathf.Deg2Rad * crankSpeed) * pedalAdjustments.crankRadius);
        }
        
        private void RotateGearStars()
        {
            CurrentRearWheelRevolution = bicycleParts.RWheel.transform.rotation.eulerAngles.x;
            if (AccelerationInput > 0 && !isAirborne && !isBunnyHopping)
            {
                crankSpeed += Mathf.Sqrt(AccelerationInput * Mathf.Abs(Mathf.DeltaAngle(CurrentRearWheelRevolution, lastFraRearWheelRevolution) * pedalAdjustments.pedalingSpeed));
                crankSpeed %= 360;
            }
            else if (Mathf.Floor(crankSpeed) > restingCrank)
                crankSpeed += -6;
            else if (Mathf.Floor(crankSpeed) < restingCrank)
                crankSpeed = Mathf.Lerp(crankSpeed, restingCrank, Time.deltaTime * 5);

            lastFraRearWheelRevolution = CurrentRearWheelRevolution;
            bicycleParts.crank.transform.localRotation = Quaternion.Euler(crankSpeed, 0, 0);
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
                
                CustomInput(inputs.Steer, ref SteerInput, 5, 5, true);
                CustomInput(inputs.Acceleration, ref AccelerationInput, 1, 1, true);
                CustomInput(inputs.Steer, ref LeanInput, 1, 1, false);
                CustomInput(inputs.Acceleration, ref rawAccelerationInput, 1, 1, true);

                _isSprinting = inputs.SprintHit;

                if (inputs.BrakesHit && isMovingBackwards == false && !isAirborne)
                {
                    Rigidbody.AddForce(-transform.forward * (accelerationCurve.Evaluate(AccelerationInput) * 0.39f)); 
                }
                
                if (WayPointSystem.recordingState == WayPointSystem.RecordingState.Record)
                {
                    if (Time.frameCount % WayPointSystem.frameIncrement == 0)
                    {
                        WayPointSystem.bicyclePositionTransform.Add(new Vector3(Mathf.Round(transform.position.x * 100f) * 0.01f, Mathf.Round(transform.position.y * 100f) * 0.01f, Mathf.Round(transform.position.z * 100f) * 0.01f));
                        WayPointSystem.bicycleRotationTransform.Add(transform.rotation);
                        WayPointSystem.movementInstructionSet.Add(new Vector2Int((int)Input.GetAxisRaw("Horizontal"), (int)Input.GetAxisRaw("Vertical")));
                        WayPointSystem.sprintInstructionSet.Add(_isSprinting);
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
                        transform.position = Vector3.Lerp(_transform.position, WayPointSystem.bicyclePositionTransform[Time.frameCount / WayPointSystem.frameIncrement], Time.deltaTime * WayPointSystem.frameIncrement);
                        transform.rotation = Quaternion.Lerp(_transform.rotation, WayPointSystem.bicycleRotationTransform[Time.frameCount / WayPointSystem.frameIncrement], Time.deltaTime * WayPointSystem.frameIncrement);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].x, ref SteerInput, 5, 5, false);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].y, ref AccelerationInput, 1, 1, false);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].x, ref LeanInput, 1, 1, false);
                        WayPointInput(WayPointSystem.movementInstructionSet[Time.frameCount / WayPointSystem.frameIncrement].y, ref rawAccelerationInput, 1, 1, true);
                        _isSprinting = WayPointSystem.sprintInstructionSet[Time.frameCount / WayPointSystem.frameIncrement];
                        bunnyHopInputState = WayPointSystem.bHopInstructionSet[Time.frameCount / WayPointSystem.frameIncrement];
                    }
                }
            }
        }
        
        private void CustomInput(float raw, ref float axis, float sensitivity, float gravity, bool isRaw)
        {
            if (isRaw)
            {
                axis = raw;
                return;
            }
            float deltaTime = Time.unscaledDeltaTime;

            if (raw >= Mathf.Epsilon)
            {
                axis = Mathf.Clamp(axis + raw * sensitivity * deltaTime, -1f, 1f);
            }
            else
            {
                axis = Mathf.Clamp01(Mathf.Abs(axis) - gravity * deltaTime) * Mathf.Sign(axis);
            }
        }

        private void WayPointInput(float instruction, ref float axis, float sensitivity, float gravity, bool isRaw)
        {
            if (isRaw)
            {
                axis = instruction;
                return;
            }
            float time = Time.unscaledDeltaTime;

            if (instruction != 0)
            {
                axis = Mathf.Clamp(axis + instruction * sensitivity * time, -1f, 1f);
            }
            else
            {
                axis = Mathf.Clamp01(Mathf.Abs(axis) - gravity * time) * Mathf.Sign(axis);
            }
        }

        private IEnumerator DelayBunnyHop()
        {
            yield return new WaitForSeconds(0.5f);
            isBunnyHopping = false;
            yield return null;
        }

        public void Pause()
        {
            Rigidbody.isKinematic = true;
            _isPaused = true;
        }

        public void Continue()
        {
            Rigidbody.isKinematic = false;
            _isPaused = false;
        }
    }
}