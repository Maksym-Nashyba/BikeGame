using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace SBPScripts
{
    public class CyclistAnimController : MonoBehaviour
    {
        private BicycleController _bicycleController;
        private BicycleStatus _bicycleStatus;
        private Animator _animator;
        private string _clipInfoCurrent, _clipInfoLast;
        
        [HideInInspector]
        public float speed;
        
        [HideInInspector]
        public GameObject hipIK, chestIK, leftFootIK, leftFootIdleIK, headIK;
        private TwoBoneIKConstraint _leftFootIK;
        private TwoBoneIKConstraint _leftFootIdleIK;

        private void Start()
        {
            _bicycleController = FindObjectOfType<BicycleController>();
            _bicycleStatus = FindObjectOfType<BicycleStatus>();
            _animator = GetComponent<Animator>();

            _leftFootIK = leftFootIK.GetComponent<TwoBoneIKConstraint>();
            _leftFootIK.weight = 0;
            _leftFootIdleIK = leftFootIdleIK.GetComponent<TwoBoneIKConstraint>();
            _leftFootIdleIK.weight = 0;
            
            chestIK.GetComponent<TwoBoneIKConstraint>().weight = 0;
            hipIK.GetComponent<MultiParentConstraint>().weight = 0;
            headIK.GetComponent<MultiAimConstraint>().weight = 0;
        }

        private void FixedUpdate()
        {
            speed = _bicycleController.transform.InverseTransformDirection(_bicycleController.Rigidbody.velocity).z;
            _animator.SetFloat("Speed", speed);
            _animator.SetBool("isAirborne", _bicycleController.isAirborne);
            
            if (_bicycleStatus != null)
            {
                if (_bicycleStatus.dislodged) return;
                
                if (!_bicycleController.isAirborne && _bicycleStatus.onBike)
                {
                    _clipInfoCurrent = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    if (_clipInfoCurrent == "IdleToStart" && _clipInfoLast == "Idle") StartCoroutine(LeftFootIK(0));
                    if (_clipInfoCurrent == "Idle" && _clipInfoLast == "IdleToStart") StartCoroutine(LeftFootIK(1));
                    if (_clipInfoCurrent == "Idle" && _clipInfoLast == "Reverse") StartCoroutine(LeftFootIdleIK(0));
                    if (_clipInfoCurrent == "Reverse" && _clipInfoLast == "Idle") StartCoroutine(LeftFootIdleIK(1));
                    _clipInfoLast = _clipInfoCurrent;
                }
            }
            else
            {
                if (_bicycleController.isAirborne) return;
                
                _clipInfoCurrent = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                if (_clipInfoCurrent == "IdleToStart" && _clipInfoLast == "Idle") 
                    StartCoroutine(LeftFootIK(0));
                if (_clipInfoCurrent == "Idle" && _clipInfoLast == "IdleToStart") 
                    StartCoroutine(LeftFootIK(1));
                if (_clipInfoCurrent == "Idle" && _clipInfoLast == "Reverse") 
                    StartCoroutine(LeftFootIdleIK(0));
                if (_clipInfoCurrent == "Reverse" && _clipInfoLast == "Idle") 
                    StartCoroutine(LeftFootIdleIK(1));
                _clipInfoLast = _clipInfoCurrent;
            }
        }

        IEnumerator LeftFootIK(int offset)
        {
            float t1 = 0f;
            while (t1 <= 1f)
            {
                t1 += Time.fixedDeltaTime;
                _leftFootIK.weight = Mathf.Lerp(-0.05f, 1.05f, Mathf.Abs(offset - t1));
                _leftFootIdleIK.weight = 1 - _leftFootIK.weight;
                yield return null;
            }
        }
        
        IEnumerator LeftFootIdleIK(int offset)
        {
            float t1 = 0f;
            while (t1 <= 1f)
            {
                t1 += Time.fixedDeltaTime;
                _leftFootIdleIK.weight = Mathf.Lerp(-0.05f, 1.05f, Mathf.Abs(offset - t1));
                yield return null;
            }
        }
    }
}