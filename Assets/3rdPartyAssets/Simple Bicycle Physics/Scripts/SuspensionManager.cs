using UnityEngine;

namespace SBPScripts
{
    public class SuspensionManager : MonoBehaviour
    {
        //Start function only.
        public bool enable;

        [Header("Joint Data")] public GameObject fSuspension;
        public GameObject rSuspension;
        [Header("Settings")] public float frontSpring;
        public float frontDamper;
        public float rearSpring;
        public float rearDamper;

        [Header("[Optional] Misc Transform Corrections")]
        public GameObject chain;

        public GameObject frontSuspensionMesh;
        public GameObject spring;
        BicycleController bicycleController;
        JointDrive fDrive, rDrive;
        SoftJointLimit fLimit;
        SoftJointLimitSpring fSpring;

        Vector3 initialSpringScale;

        void Start()
        {
            bicycleController = FindObjectOfType<BicycleController>();
            //Suspension Settings
            if (enable)
            {
                if (fSuspension != null)
                {
                    fSuspension.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Limited;

                    fLimit.limit = 0.01f;
                    fSuspension.GetComponent<ConfigurableJoint>().linearLimit = fLimit;

                    fSpring.spring = 10000;
                    fSpring.damper = 500;
                    fSuspension.GetComponent<ConfigurableJoint>().linearLimitSpring = fSpring;

                    fDrive.positionSpring = frontSpring;
                    fDrive.positionDamper = frontDamper;
                    fDrive.maximumForce = Mathf.Infinity;
                    fSuspension.GetComponent<ConfigurableJoint>().yDrive = fDrive;
                }

                if (rSuspension != null)
                {
                    rSuspension.GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Free;

                    rDrive.positionSpring = rearSpring;
                    rDrive.positionDamper = rearDamper;
                    rDrive.maximumForce = Mathf.Infinity;
                    rSuspension.GetComponent<ConfigurableJoint>().angularXDrive = rDrive;
                }
            }
            else
            {
                if (fSuspension != null)
                    fSuspension.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Locked;
                if (rSuspension != null)
                    rSuspension.GetComponent<ConfigurableJoint>().angularXMotion = ConfigurableJointMotion.Locked;
            }

            if (spring != null)
                initialSpringScale = spring.transform.localScale;
        }

        void Update()
        {
            if (!enable) return;

            Vector3 position = bicycleController.fPhysicsWheel.transform.position;
            bicycleController.bicycleParts.fWheelVisual.transform.position = new Vector3(
                position.x,
                position.y,
                position.z);
            if (chain != null) chain.transform.rotation = rSuspension.transform.rotation;
            if (frontSuspensionMesh != null)
                frontSuspensionMesh.transform.rotation = bicycleController.bicycleParts.lowerFork.transform.rotation;
            if (spring != null && rSuspension != null)
                if (rSuspension.transform.eulerAngles.x is > 0 and < 20)
                    spring.transform.localScale = new Vector3(initialSpringScale.x,
                        initialSpringScale.y - Mathf.Clamp01(rSuspension.transform.eulerAngles.x / 20),
                        initialSpringScale.z);
        }
    }
}