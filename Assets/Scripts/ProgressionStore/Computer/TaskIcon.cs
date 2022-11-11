using System;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class TaskIcon : MonoBehaviour
    {
        public event Action<Program> Clicked;
        private Program _program;
        
        public void SetUp(Program program)
        {
            _program = program;
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        public void OnClicked()
        {
            Clicked?.Invoke(_program);
        }
    }
}