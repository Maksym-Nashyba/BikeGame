using System;
using UnityEngine;

namespace LevelObjectives
{
    public class Checkpoint : MonoBehaviour
    {
        public event Action Activated;

        private void OnTriggerEnter(Collider other)
        {
            Activated?.Invoke();
        }
    }
}