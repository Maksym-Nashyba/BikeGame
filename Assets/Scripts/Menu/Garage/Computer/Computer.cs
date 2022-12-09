using System;
using System.Collections.Generic;
using System.Linq;
using ProgressionStore.Computer;
using UnityEngine;

namespace Menu.Garage.Computer
{
    public class Computer : MonoBehaviour
    {
        public event Action<Program> ProgramLaunched;
        public event Action<Program> ProgramTerminated;
        
        [SerializeField] private Program[] _programs;
        [SerializeField] private TaskBar _taskBar;
        [Space] 
        [SerializeField] private GameObject _dialogWindowPrefab;
        [SerializeField] private CanvasInputSimulator _inputSimulator;
        [SerializeField] private MeshClickListener _screenClickListener;

        private List<Program> _runningProcesses;
        private LinkedList<Window> _openWindows; //First is top (no, I couldn't have used a stack)

        private void Awake()
        {
            _runningProcesses = new List<Program>();
            _openWindows = new LinkedList<Window>();
            
            _screenClickListener.ClickedUV += _inputSimulator.ClickAtUV;
        }

        public Program[] ProgramsCopy => (Program[])_programs.Clone(); 
        
        public void Launch(Program program)
        {
            if(_runningProcesses.Contains(program))
            {
                OpenWindow(program);
                return;
            }
            
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
        }

        public void OpenWindow(Program program)
        {
            OpenWindow(FindWindow(program));
        }

        public void ShowDialog(string message)
        {
            
        }
        
        private void OpenWindow(Window window)
        {
            _openWindows.Remove(window);
            _openWindows.AddFirst(window);
            window.Open();
        }

        private void CloseWindow(Window window)
        {
            _openWindows.Remove(window);
            
            window.HideButtonPressed -= HideWindow;
            window.CloseButtonPressed -= Terminate;
            window.Close();
        }

        private void HideWindow(Window window)
        {
            window.Hide();
            _openWindows.Remove(window);
            _openWindows.AddLast(window);
        }

        public void HideAllWindows()
        {
            Window[] windows = _openWindows.ToArray();
            foreach (Window window in windows)
            {
                HideWindow(window);
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
            windowTransform.SetSiblingIndex(_taskBar.transform.GetSiblingIndex());
            Window window = windowTransform.GetComponent<Window>();
            window.SetUp(program);
            window.HideButtonPressed += HideWindow;
            window.CloseButtonPressed += Terminate;
            window.Hide();
            _openWindows.AddLast(window);
        }
        
        private void OnDestroy()
        {
            _screenClickListener.ClickedUV -= _inputSimulator.ClickAtUV;
        }
    }
}