using System.Collections.Generic;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class TaskBar : MonoBehaviour
    {
        [SerializeField] private GameObject _taskIconPrefab;
        private List<Window> _runningProcesses = new List<Window>();

        public void StartProcess(Window window)
        {
            if(_runningProcesses.Contains(window))return;
            
            _runningProcesses.Add(window);
        }
    }
}