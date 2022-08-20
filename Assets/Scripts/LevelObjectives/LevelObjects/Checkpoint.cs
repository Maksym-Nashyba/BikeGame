using UnityEngine;

namespace LevelObjectives.LevelObjects
{
    public class Checkpoint : PlayerTrigger
    {
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
        }
    }
}