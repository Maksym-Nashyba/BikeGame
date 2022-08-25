using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SetUp
{
    public class GameSetUp : MonoBehaviour
    {
        internal event Action FinishedSetUp;
        internal bool RegistrationOpen => !_startedSetUp;
        private List<Task> _setUpTasks = new List<Task>();
        private bool _startedSetUp;

        private void Awake()
        {
            FinishedSetUp += OnSetUpPerformed;
        }

        private async void Update()
        {
            if (!_startedSetUp)
            {
                _startedSetUp = true;
                await PerformSetUp();
            }
        }

        internal void RegisterSetUpTask(Task task)
        {
            if (!RegistrationOpen) throw new Exception("Start up tasks should be registered in Start/Awake");
            
            _setUpTasks.Add(task);
        }

        private async Task PerformSetUp()
        {
            foreach (Task task in _setUpTasks)
            {
                await task;
            }
            FinishedSetUp?.Invoke();
        }

        private void OnSetUpPerformed()
        {
            FinishedSetUp -= OnSetUpPerformed;
            enabled = false;
        }
    }
}