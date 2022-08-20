using Misc;
using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    public class Checkpoint : PlayerTrigger
    {
        private Transformation _respawnPoint;
        
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            _respawnPoint = new Transformation(other.transform);
        }

        public Transformation GetRespawnTransformation()
        {
            return _respawnPoint;
        }
    }
}