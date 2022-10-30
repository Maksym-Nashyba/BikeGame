using System.Collections;
using System.Security.Cryptography;
using Misc;
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
        private bool _prevOnBike;
        private bool _prevDislodged;
        public GameObject inactiveColliders;
        private BicycleController _bicycleController;
        private Rigidbody _rigidbody;

        void Start()
        {
            _bicycleController = GetComponent<BicycleController>();
            _rigidbody = GetComponent<Rigidbody>();
            if (onBike)
            {
                 StartCoroutine(BikeStand(1));
            }
            else
            {
                StartCoroutine(BikeStand(0));
            }

            ServiceLocator.Player.Died += Dislodge;
        }
        
        void OnCollisionEnter(Collision collision)
        {
            //Detects if there is a ragdoll to instantiate in the first place along with collsion impact detection
            if (collision.relativeVelocity.magnitude > impactThreshold && ragdollPrefab != null)
            {
                dislodged = true;
            }
        }

        void Update()
        {
            if(dislodged != _prevDislodged)
            {
                if(dislodged)
                { 
                    ServiceLocator.Player.Die();
                }
                else
                {
                    Lodge();
                }
            }
            
            _prevDislodged = dislodged;
            
            if (onBike != _prevOnBike)
            {
                if (onBike && dislodged == false)
                {
                    StartCoroutine(BikeStand(1));
                }
                else
                {
                    StartCoroutine(BikeStand(0));
                }
            }
            _prevOnBike = onBike;
        }

        private void Dislodge()
        {
            _bicycleController.fPhysicsWheel.GetComponent<SphereCollider>().enabled = false;
            _bicycleController.rPhysicsWheel.GetComponent<SphereCollider>().enabled = false;
            _bicycleController.Rigidbody.centerOfMass = _bicycleController.GetComponent<BoxCollider>().center;
            _bicycleController.enabled = false;
            inactiveColliders.SetActive(true);
            instantiatedRagdoll = Instantiate(ragdollPrefab);
            Destroy(this);
        }

        private void Lodge()
        {
            _bicycleController.fPhysicsWheel.GetComponent<SphereCollider>().enabled = true;
            _bicycleController.rPhysicsWheel.GetComponent<SphereCollider>().enabled = true;
            _bicycleController.enabled = true;
            _bicycleController.Rigidbody.centerOfMass = _bicycleController.centerOfMassOffset;
            inactiveColliders.SetActive(false);
            Destroy(instantiatedRagdoll);
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
                _bicycleController.enabled = true;
                _rigidbody.constraints = RigidbodyConstraints.None;
            }

            if (instruction == 0)
            {
                float t = 0f;
                while (t <= 1)
                {
                    t += Time.deltaTime * 5;
                    _bicycleController.SteerInput = -Mathf.Abs(instruction - t);
                    yield return null;
                }
                _bicycleController.enabled = false;
                yield return new WaitForSeconds(1);
                
                float l = 0f;
                while (l <= 1)
                {
                    l += Time.deltaTime * 5;
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, l*5);
                    yield return null;
                }
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 5);
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private void OnDestroy()
        {
            ServiceLocator.Player.Died -= Dislodge;
        }
    }
}