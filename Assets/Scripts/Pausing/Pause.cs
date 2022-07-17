using System.Linq;
using UnityEngine;

namespace Pausing
{
    public class Pause : MonoBehaviour
    {
        public State CurrentState { get; private set; }

        private void Start()
        {
            CurrentState = State.Playing;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseAll();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                ContinueAll();
            }
        }
        
        public void PauseAll()
        {
            IPausable[] pausableObjects = FindAllPausableObjects();
            foreach (IPausable pausable in pausableObjects)
            {
                pausable.Pause();
            }
            CurrentState = State.Paused;
        }
        
        public void ContinueAll()
        {
            IPausable[] pausableObjects = FindAllPausableObjects();
            foreach (IPausable pausable in pausableObjects)
            {
                pausable.Continue();
            }
            CurrentState = State.Playing;
        }

        private IPausable[] FindAllPausableObjects()
        { 
            return FindObjectsOfType<MonoBehaviour>().OfType<IPausable>().ToArray();
        }
    }
}