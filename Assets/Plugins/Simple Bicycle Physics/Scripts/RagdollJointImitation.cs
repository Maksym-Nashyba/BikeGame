using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBPScripts{
public class RagdollJointImitation : MonoBehaviour
{

    private string limbTag = "TrackedLimb";
    GameObject CharacterCopyPosition;
    GameObject CharacterCopyAnimations;
    BicycleController bicycleController;

    private Transform animRoot;
    public int bodyPartMass = 1;

    public float jointAngularDamping = 100f;
    public float Stiffness = 1000f;
    
    public float launchVelocityMultiplier = 1;

    public Transform[] allAnimTrans;
    public ConfigurableJoint[] confJoints;


    void Start()
    {
        bicycleController = FindObjectOfType<BicycleController>();
        CharacterCopyPosition = bicycleController.GetComponentInChildren<CyclistAnimController>(true).gameObject;
        CharacterCopyAnimations = GameObject.FindWithTag("AnimationToFollow");
        transform.position = new Vector3(CharacterCopyPosition.transform.position.x,bicycleController.transform.position.y,CharacterCopyPosition.transform.position.z);
        transform.rotation = CharacterCopyPosition.transform.rotation;
        PopulateArrays();
        AddJointFollowScript();
    }

    public void CopyInitialPositions()
    {
        foreach (Transform trans in transform.GetComponentsInChildren<Transform>())
        {
            if (trans.GetComponent<ConfigurableJoint>() != null || trans.name == "mixamorig:Hips")
            {
                foreach (Transform rider in CharacterCopyPosition.transform.GetComponentsInChildren<Transform>())
                {
                    if (trans.name == rider.name)
                        trans.localRotation = rider.localRotation;
                }
            }
        }
        foreach (Transform trans in transform.GetComponentsInChildren<Transform>())
        {
            if (trans.GetComponent<ConfigurableJoint>() != null)
            {
                foreach (Transform animRider in CharacterCopyAnimations.transform.GetComponentsInChildren<Transform>())
                {
                    if (trans.name == animRider.name)
                        animRider.localRotation = trans.localRotation;
                }
            }
        }


    }

    private void PopulateArrays()
    {
        animRoot = CharacterCopyAnimations.transform.Find("mixamorig:Hips");
        Transform[] animTransArr = animRoot.GetComponentsInChildren<Transform>();
        Transform[] ragTransArr = transform.GetComponentsInChildren<Transform>();
        List<Transform> transList = new List<Transform>();
        List<ConfigurableJoint> jointList = new List<ConfigurableJoint>();

        foreach (Transform trans in animTransArr)
        {
            if (trans.tag == limbTag)
            {
                transList.Add(trans);
            }
        }
        allAnimTrans = transList.ToArray();

        foreach (Transform trans in ragTransArr)
        {
            ConfigurableJoint cj = trans.GetComponent<ConfigurableJoint>();
            if (cj != null)
            {
                //default contact to 0.1, max depenetration to 0.1 Fixed TimeScale to  0.01
                jointList.Add(cj);
                cj.projectionMode = JointProjectionMode.PositionAndRotation;
                cj.projectionDistance = 5f;
                cj.projectionAngle = 5f;
                cj.enablePreprocessing = false;
                trans.GetComponent<Rigidbody>().solverIterations = 4;
                trans.GetComponent<Rigidbody>().mass = bodyPartMass;
                trans.GetComponent<Rigidbody>().velocity = bicycleController.GetComponent<Rigidbody>().velocity * launchVelocityMultiplier;
                trans.GetComponent<Rigidbody>().maxAngularVelocity = Mathf.Infinity;
            }
        }
        confJoints = jointList.ToArray();
    }

    private void AddJointFollowScript()
    {
        foreach (ConfigurableJoint cj in confJoints)
        {
            cj.gameObject.AddComponent<RagdollJointConfig>();
            //cj.connectedBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            for (int t = 0; t < allAnimTrans.Length; t++)
            {
                if (allAnimTrans[t].name == cj.gameObject.name)
                {
                    cj.GetComponent<RagdollJointConfig>().torqueForce = Stiffness;
                    cj.GetComponent<RagdollJointConfig>().angularDamping = jointAngularDamping;
                    cj.GetComponent<RagdollJointConfig>().target = allAnimTrans[t];
                }
            }
        }
    }

}
}
