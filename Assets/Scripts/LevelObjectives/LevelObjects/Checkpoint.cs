using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    public class Checkpoint : PlayerTrigger
    {
        private Vector3 _respawnPoint;
        
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            _respawnPoint = other.transform.position;
        }

        public Vector3 GetRespawnPoint()
        {
            return _respawnPoint;
        }
    }
}