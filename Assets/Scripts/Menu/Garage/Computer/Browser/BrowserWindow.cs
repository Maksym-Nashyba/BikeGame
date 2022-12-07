using System;
using UnityEngine;

namespace ProgressionStore.Computer.Browser
{
    public class BrowserWindow : Window
    {
        [SerializeField] private BrowserTab[] _tabs;

        private void Start()
        {
            _tabs[0].Select();
        }

        protected override void OnOpened()
        {
            foreach (BrowserTab tab in _tabs)
            {
                tab.Selected += OnTabSelected;
            }
        }

        protected override void OnClosed()
        {
            foreach (BrowserTab tab in _tabs)
            {
                tab.Selected -= OnTabSelected;
            }
        }
        
        private void OnTabSelected(BrowserTab tab)
        {
            foreach (BrowserTab browserTab in _tabs)
            {
                if(tab == browserTab) continue;
                browserTab.UnSelect();
            }
        }
    }
}