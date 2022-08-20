using System;
using Misc;
using UnityEngine;

namespace LevelObjectives
{
    public class Checkpoint : MonoBehaviour
    {
        public event Action Activated;
        private Transform _playerTransform;

        private void OnTriggerEnter(Collider other)
        {
            _playerTransform = other.transform;
            ServiceLocator.PlayerSpawner.UnlockCheckpoint(this);
            Activated?.Invoke();
        }

        public Transform GetPlayerTransform()
        {
            return _playerTransform;
        }
    }
}