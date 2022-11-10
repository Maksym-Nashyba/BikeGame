using UnityEngine;

namespace ProgressionStore.Computer
{
    public class ComputerUI : MonoBehaviour
    {
        [SerializeField] private CanvasInputSimulator _inputSimulator;
        [SerializeField] private MeshClickListener _screenClickListener;

        private Window _currentWindow;
        
        private void Awake()
        {
            _screenClickListener.ClickedUV += _inputSimulator.ClickAtUV;
        }
        
        
        
        private void OnDestroy()
        {
            _screenClickListener.ClickedUV -= _inputSimulator.ClickAtUV;
        }
    }
}