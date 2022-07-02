using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SBPScripts
{
    public class BicycleStatus : MonoBehaviour
    {
        public bool onBike = true;
        public bool dislodged;
        public float impactThreshold;
        public GameObject ragdollPrefab;
        [HideInInspector]
        public GameObject instantiatedRagdoll;
        bool prevOnBike, prevDislodged;
        public GameObject inactiveColliders;
        BicycleController bicycleController;
        Rigidbody rb;
        void Start()
        {
            bicycleController = GetComponent<BicycleController>();
            rb = GetComponent<Rigidbody>();
            if (onBike)
                    StartCoroutine(BikeStand(1));
                else
                    StartCoroutine(BikeStand(0));

        }
        void OnCollisionEnter(Collision collision)
        {
            //Detects if there is a ragdoll to instantiate in the first place along with collsion impact detection
            if (collision.relativeVelocity.magnitude > impactThreshold && ragdollPrefab!=null)
                dislodged = true;
        }
        void Update()
        {
            if(dislodged != prevDislodged)
            {
                if(dislodged)
                {
                    bicycleController.fPhysicsWheel.GetComponent<SphereCollider>().enabled = false;
                    bicycleController.rPhysicsWheel.GetComponent<SphereCollider>().enabled = false;
                    bicycleController.rb.centerOfMass = bicycleController.GetComponent<BoxCollider>().center;
                    bicycleController.enabled = false;
                    inactiveColliders.SetActive(true);
                    instantiatedRagdoll = Instantiate(ragdollPrefab);
                }
                else
                {
                    bicycleController.fPhysicsWheel.GetComponent<SphereCollider>().enabled = true;
                    bicycleController.rPhysicsWheel.GetComponent<SphereCollider>().enabled = true;
                    bicycleController.enabled = true;
                    bicycleController.rb.centerOfMass = bicycleController.centerOfMassOffset;
                    inactiveColliders.SetActive(false);
                    Destroy(instantiatedRagdoll);
                }
            }
            prevDislodged = dislodged;
            if (onBike != prevOnBike)
            {
                if (onBike && dislodged == false)
                    StartCoroutine(BikeStand(1));
                else
                    StartCoroutine(BikeStand(0));
            }
            prevOnBike = onBike;


        }

        IEnumerator BikeStand(int instruction)
        {
            
            if (instruction == 1)
            {
                
                float t = 0f;
                while (t <= 1)
                {
                    t += Time.deltaTime * 5;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0),t);
                    yield return null;
                }
                bicycleController.enabled = true;
                rb.constraints = RigidbodyConstraints.None;
                
                
            }

            if (instruction == 0)
            {
                
                float t = 0f;
                while (t <= 1)
                {
                    t += Time.deltaTime * 5;
                    bicycleController.customSteerAxis = -Mathf.Abs(instruction - t);
                    yield return null;
                }
                bicycleController.enabled = false;
                yield return new WaitForSeconds(1);
                float l = 0f;
                while (l <= 1)
                {
                    l += Time.deltaTime * 5;
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, l*5);
                    yield return null;
                }
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 5);
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }

        }
    }
}
