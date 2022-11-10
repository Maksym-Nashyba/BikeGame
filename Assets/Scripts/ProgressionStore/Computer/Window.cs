using UnityEngine;

namespace ProgressionStore.Computer
{
    public abstract class Window : MonoBehaviour
    {
        public Program Program => _program;
        [SerializeField] private Program _program;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}