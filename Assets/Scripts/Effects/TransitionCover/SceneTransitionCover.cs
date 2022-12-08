using System;
using System.Threading.Tasks;
using Misc;
using UnityEngine;

namespace Effects.TransitionCover
{
    public abstract class SceneTransitionCover : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        public event Action<State> ReachedState;
        
        [SerializeField] protected State StartState;
        [SerializeField] protected bool TransitionOnStart;
        [SerializeField] protected float TransitionDurationSeconds;
        
        protected AsyncExecutor AsyncExecutor { get; private set; }

        protected virtual void Awake()
        {
            CurrentState = State.None;
            TransitionStateInstantly(StartState);
            AsyncExecutor = new AsyncExecutor();
        }

        private void Start()
        {
            if (TransitionOnStart)
            {
                State opposite = StartState == State.Clean ? State.Covered : State.Clean;
                TransitionToState(opposite);
            }
        }

        private void OnDestroy()
        {
            AsyncExecutor.Dispose();
        }

        public void TransitionStateInstantly(State targetState)
        {
            if (CurrentState == targetState) throw new InvalidOperationException($"State is already {targetState}");
            PlayTransitionImmediate(targetState);
            SetState(targetState);
        }

        public async Task TransitionToState(State targetState)
        {
            if (CurrentState == targetState) throw new InvalidOperationException($"State is already {targetState}");
            await PlayTransitionAnimation(targetState);
            SetState(targetState);
        }
        
        protected abstract Task PlayTransitionAnimation(State targetState);
        
        protected abstract void PlayTransitionImmediate(State targetState);
        
        
        protected void SetState(State targetState)
        {
            CurrentState = targetState;
            ReachedState?.Invoke(targetState);
        }
        
        public enum State
        {
            None,
            Covered,
            Clean
        }
    }
}