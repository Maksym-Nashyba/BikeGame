using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProgressionStore.Computer
{
    public class DesktopIcon : MonoBehaviour
    {
        public event Action<Program> Clicked;
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _iconName;
        private Program _program;
        
        public void SetUp(Program program)
        {
            _program = program;
            _iconImage.sprite = program.TaskBarProcessSprite;
            _iconName.SetText(program.PresentableName);
        }

        public void OnClicked()
        {
            Clicked?.Invoke(_program);
        }
    }
}