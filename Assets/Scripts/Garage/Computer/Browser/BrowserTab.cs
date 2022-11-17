using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProgressionStore.Computer.Browser
{
    public class BrowserTab : MonoBehaviour
    {
        public event Action<BrowserTab> Selected;
        [SerializeField] private GameObject _window;
        [SerializeField] private Image _tabBackground;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _inActiveColor;

        private void Awake()
        {
            UnSelect();
        }

        public void Select()
        {
            Selected?.Invoke(this);
            _tabBackground.color = _activeColor;
            _window.SetActive(true);
        }

        public void UnSelect()
        {
            _tabBackground.color = _inActiveColor;
            _window.SetActive(false);
        }
    }
}