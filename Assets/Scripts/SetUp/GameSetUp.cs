using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SaveSystem.Front;
using UnityEngine;

namespace SetUp
{
    public class GameSetUp : MonoBehaviour
    {
        internal event Action FinishedSetUp;
        internal bool RegistrationOpen => !StartedSetUp;
        private bool StartedSetUp;
        private List<Task> SetUpTasks;

        private void Awake()
        {
            SetUpTasks = new List<Task>();
            RegisterSetUpTask(FindObjectOfType<SavesInitializer>().InitializeOnDemand());
        }

        private async void Update()
        {
            if (!StartedSetUp)
            {
                StartedSetUp = true;
                await PerformSetUp();
            }
        }

        internal void RegisterSetUpTask(Task task)
        {
            if (!RegistrationOpen) throw new Exception("Start up tasks should be registered in Start/Awake");
            
            SetUpTasks.Add(task);
        }

        private async Task PerformSetUp()
        {
            foreach (Task task in SetUpTasks)
            {
                await task;
            }
            FinishedSetUp?.Invoke();
            Debug.Log(FindObjectOfType<Saves>().Bikes.GetAllUnlockedBikes()[0].GUID);
        }
    }
}