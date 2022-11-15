using System.Collections.Generic;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class TaskBar : MonoBehaviour
    {
        [SerializeField] private ComputerUI _computerUI;
        [SerializeField] private Transform _taskHolder;
        [SerializeField] private GameObject _taskIconPrefab;

        private Dictionary<Program, TaskIcon> _tasks;

        private void Awake()
        {
            _tasks = new Dictionary<Program, TaskIcon>();
            _computerUI.ProgramLaunched += OnProgramLaunched;
            _computerUI.ProgramTerminated += OnProgramTerminated;
        }

        private void OnProgramLaunched(Program program)
        {
            CreateTaskIcon(program);
        }
        
        private void OnProgramTerminated(Program program)
        {
            RemoveTaskIcon(program);
        }

        public void OnStartButton()
        {
            _computerUI.HideAllWindows();
        }
        
        private void CreateTaskIcon(Program program)
        {
            TaskIcon taskIcon = Instantiate(_taskIconPrefab, _taskHolder).GetComponent<TaskIcon>();
            taskIcon.SetUp(program);
            taskIcon.Clicked += _computerUI.OpenWindow;
            _tasks.Add(program, taskIcon);
        }

        private void RemoveTaskIcon(Program program)
        {
            _tasks[program].Clicked -= _computerUI.OpenWindow;
            _tasks[program].Close();
            _tasks.Remove(program);
        }
    }
}