using System;
using UnityEngine;

namespace LevelLoading
{
    public class SceneTransitionCover : MonoBehaviour
    {
        public State CurrentState { get; private set; }
        public event Action<State> ReachedState;
        
        [SerializeField] protected State StartState;
        [SerializeField] protected bool FireOnStart;
        [SerializeField] protected float TransitionDurationSeconds;
        
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