using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBPScripts
{
    public class TPSCamSwitch : MonoBehaviour
    {
        public GameObject cyclist;
        public GameObject externalCharacter;
        BicycleCamera bicycleCamera;
        BicycleStatus bicycleStatus;
        void Start()
        {
            bicycleCamera = FindObjectOfType<BicycleCamera>();
            bicycleStatus = FindObjectOfType<BicycleStatus>();
        }
        void LateUpdate()
        {
            if (externalCharacter != null)
            {
                if (externalCharacter.activeInHierarchy)
                {
                    bicycleCamera.target = externalCharacter.transform;
                }
                else
            {
                bicycleCamera.target = cyclist.transform.root.transform;
            }
            }
            
            
            if (bicycleStatus.dislodged && bicycleStatus.instantiatedRagdoll!=null)
            {
                bicycleCamera.target = bicycleStatus.instantiatedRagdoll.transform.Find("mixamorig:Hips").gameObject.transform;
            }
            else if(externalCharacter==null)
            {
                bicycleCamera.target = cyclist.transform.root.transform;
            }
        }
    }
}
