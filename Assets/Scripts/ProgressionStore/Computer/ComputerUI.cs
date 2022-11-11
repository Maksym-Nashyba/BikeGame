using System;
using System.Collections.Generic;
using System.Linq;
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
        private LinkedList<Window> _openWindows; //First is top

        private void Awake()
        {
            _runningProcesses = new List<Program>();
            _openWindows = new LinkedList<Window>();
            
            _screenClickListener.ClickedUV += _inputSimulator.ClickAtUV;
        }
        
        private void Start()
        {
            Launch(_programs[0]);
        }

        public void Launch(Program program)
        {
            if(_runningProcesses.Contains(program))return;
            
            _runningProcesses.Add(program);
            CreateWindow(program);
            OpenWindow(FindWindow(program));
            ProgramLaunched?.Invoke(program);
        }

        public void Terminate(Program program)
        {
            if (!_runningProcesses.Contains(program)) throw new InvalidOperationException($"Process [{program.PresentableName}] isn't launched and can't be terminated");
            
            CloseWindow(FindWindow(program));
            ProgramTerminated?.Invoke(program);
            _runningProcesses.Remove(program);
            OpenTopWindow();
        }

        private void OpenWindow(Window window)
        {
            _openWindows.Remove(window);
            _openWindows.AddFirst(window);
            window.transform.SetSiblingIndex(_taskBar.transform.GetSiblingIndex());
            window.Open();
        }

        private void CloseWindow(Window window)
        {
            _openWindows.Remove(window);
            window.Close();
        }

        private void OpenTopWindow()
        {
            if (_openWindows.First != null)
            {
                OpenWindow(_openWindows.First.Value);
            }
        }
        
        private Window FindWindow(Program program)
        {
            if (!_runningProcesses.Contains(program)) throw new InvalidOperationException($"Process [{program.PresentableName}] isn't launched, can't open window for it");
            return _openWindows.First(x => x.Program == program);
        }
        
        private void CreateWindow(Program program)
        {
            Transform windowTransform = Instantiate(program.WindowPrefab, Vector3.zero, Quaternion.identity, transform).transform;
            windowTransform.localPosition = Vector3.zero;
            windowTransform.SetSiblingIndex(_taskBar.transform.GetSiblingIndex()+1);
            Window window = windowTransform.GetComponent<Window>();
            window.Hide();
            _openWindows.AddLast(window);
        }
        
        private void OnDestroy()
        {
            _screenClickListener.ClickedUV -= _inputSimulator.ClickAtUV;
        }
    }
}