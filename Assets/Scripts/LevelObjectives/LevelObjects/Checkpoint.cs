using UnityEngine;
using UnityEngine.Events;

namespace LevelObjectives.LevelObjects
{
    public class Checkpoint : PlayerTrigger
    {
        [SerializeField] private UnityEvent _checkpointActivated;

        protected override void Awake()
        {
            base.Awake();
            Activated += OnCheckpointActivated;
        }

        private void OnCheckpointActivated()
        {
            Activated -= OnCheckpointActivated;
            _checkpointActivated.Invoke();
        }
    }
}