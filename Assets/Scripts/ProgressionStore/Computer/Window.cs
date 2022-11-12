using System;
using TMPro;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public abstract class Window : MonoBehaviour
    {
        public event Action<Window> HideButtonPressed;
        public event Action<Program> CloseButtonPressed;
        public Program Program { get; private set; }
        public bool IsOpen { get; private set; }

        [SerializeField] private TextMeshProUGUI _windowNameText;
        
        public void SetUp(Program program)
        {
            Program = program;
            _windowNameText.SetText(program.PresentableName);
        }
        
        public void Open()
        {
            if(IsOpen) return;
            IsOpen = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if(!IsOpen) return;
            IsOpen = false;
            gameObject.SetActive(false);
        }
        
        public void Close()
        {
            IsOpen = false;
            Destroy(gameObject);
        }

        public void OnCloseButton()
        {
            CloseButtonPressed?.Invoke(Program);
        }

        public void OnHideButton()
        {
            HideButtonPressed?.Invoke(this);
        }
    }
}