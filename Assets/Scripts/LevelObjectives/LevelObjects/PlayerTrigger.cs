using System;
using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerTrigger : MonoBehaviour
    {
        public event Action Activated;
        
        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerTrigger");
            GetComponent<Collider>().isTrigger = true;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            Activated?.Invoke();
        }
    }
}