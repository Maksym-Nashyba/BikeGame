using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class ComputerUI : MonoBehaviour
    {
        public event Action<Program> ProgramLaunched;
        public event Action<Program> ProgramTerminated;
        
        [SerializeField] private Program[] _programs;
        [SerializeField] private TaskBar _taskBar;
        [Space]
        [SerializeField] private CanvasInputSimulator _inputSimulator;
        [SerializeField] private MeshClickListener _screenClickListener;

        private List<Program> _runningProcesses;
        private Stack<Window> _openWindows;

        private void Awake()
        {
            _runningProcesses = new List<Program>();
            _openWindows = new Stack<Window>();
            Launch(_programs[0]);
            _screenClickListener.ClickedUV += _inputSimulator.ClickAtUV;
        }

        private void Launch(Program program)
        {
            throw new NotImplementedException();
        }

        private void Terminate(Program program)
        {
            throw new NotImplementedException();
        }
        
        private void OpenWindow(Program program)
        {
            throw new NotImplementedException();
        }

        private Window CreateWindow(Program program)
        {
            Transform windowTransform = Instantiate(program.WindowPrefab).transform;
            windowTransform.SetParent(transform);
            windowTransform.SetSiblingIndex(_taskBar.transform.GetSiblingIndex()+1);
            Window window = windowTransform.GetComponent<Window>();
            window.Close();
            return window;
        }
        
        private void CloseWindow(Window window)
        {
            throw new NotImplementedException();
        }
        
        private void OnDestroy()
        {
            _screenClickListener.ClickedUV -= _inputSimulator.ClickAtUV;
        }
    }
}