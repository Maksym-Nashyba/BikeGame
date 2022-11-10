using System;
using UnityEngine;

namespace ProgressionStore.Computer
{
    public class WindowBorder : MonoBehaviour
    {
        public event Action CloseButtonClicked;
        public bool IsShown => gameObject.activeInHierarchy;
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnCloseButton()
        {
            CloseButtonClicked?.Invoke();
        }
    }
}