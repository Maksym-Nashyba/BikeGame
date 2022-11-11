using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProgressionStore.Computer
{
    public class TaskIcon : MonoBehaviour
    {
        public event Action<Program> Clicked;
        [SerializeField] private Image _iconImage;
        private Program _program;
        
        public void SetUp(Program program)
        {
            _program = program;
            _iconImage.sprite = program.TaskBarProcessSprite;
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