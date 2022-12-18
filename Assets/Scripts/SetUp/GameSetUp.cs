using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SetUp
{
    public class GameSetUp : MonoBehaviour
    {
        internal event Action FinishedSetUp;
        internal bool RegistrationOpen => !_startedSetUp;
        private List<SetUpOperation> _setUpOperations = new List<SetUpOperation>();
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
                SceneManager.LoadScene("MainMenu");
            }
        }

        internal void RegisterSetUpTask(SetUpOperation setUpOperation)
        {
            if (!RegistrationOpen) throw new Exception("Start up tasks should be registered in Start/Awake");
            
            _setUpOperations.Add(setUpOperation);
        }

        private async Task PerformSetUp()
        {
            foreach (SetUpOperation setUpOperation in _setUpOperations)
            {
                try
                {
                    await setUpOperation.Task();
                    Debug.Log(setUpOperation.DoneMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if(setUpOperation.IsCritical) throw;  
                }
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