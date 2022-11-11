using UnityEngine;

namespace ProgressionStore.Computer
{
    public abstract class Window : MonoBehaviour
    {
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
    }
}