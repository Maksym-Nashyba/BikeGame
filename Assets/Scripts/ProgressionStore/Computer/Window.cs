using System;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public abstract class Window : MonoBehaviour
    {
        public event Action<Window> HideButtonPressed;
        public event Action<Program> CloseButtonPressed;
        public Program Program => _program;
        [SerializeField] private Program _program;

        public bool IsOpen { get; private set; }
        
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
            CloseButtonPressed?.Invoke(_program);
        }

        public void OnHideButton()
        {
            HideButtonPressed?.Invoke(this);
        }
    }
}