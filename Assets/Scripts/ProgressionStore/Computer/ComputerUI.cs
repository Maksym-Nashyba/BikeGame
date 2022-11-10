using UnityEngine;

namespace ProgressionStore.Computer
{
    public class ComputerUI : MonoBehaviour
    {
        [SerializeField] private Window[] _windows;
        [SerializeField] private WindowBorder _border;
        [SerializeField] private TaskBar _taskBar;
        [Space]
        [SerializeField] private CanvasInputSimulator _inputSimulator;
        [SerializeField] private MeshClickListener _screenClickListener;

        private Window _currentWindow;
        
        private void Awake()
        {
            OpenWindow(_windows[0]);
            _screenClickListener.ClickedUV += _inputSimulator.ClickAtUV;
            _border.CloseButtonClicked += CloseCurrentWindow;
        }

        private void OpenWindow(Window window)
        {
            if (_currentWindow == null) _border.Show();
            window.Open();
            _taskBar.StartProcess(window);
            _currentWindow = window;
        }

        private void CloseCurrentWindow()
        {
            _currentWindow.Close();
            _border.Hide();
            _currentWindow = null;
        }
        
        private void OnDestroy()
        {
            _screenClickListener.ClickedUV -= _inputSimulator.ClickAtUV;
            _border.CloseButtonClicked -= CloseCurrentWindow;
        }
    }
}