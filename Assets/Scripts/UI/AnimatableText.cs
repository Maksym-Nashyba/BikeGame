using System;
using System.Threading.Tasks;
using Misc;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatableText : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private Transform _transform;
        private State _defaultState;
        private AsyncExecutor _executor;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _transform = GetComponent<Transform>();
            _executor = new AsyncExecutor();
        }

        private void OnDestroy()
        {
            _executor.Dispose();
        }

        private void Start()
        {
            _defaultState = GetCurrentState();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }

        public Task ReturnToDefault()
        {
            return LerpToState(_defaultState);
        }

        private Task LerpToState(State state)
        {
            return Task.WhenAll();
        }

        private Task LerpColor(Color target, float durationSeconds)
        {
            throw new NotImplementedException();
        }

        private State GetCurrentState()
        {
            return new State(
                _text.color,
                _transform.localScale,
                _transform.position,
                _transform.rotation);
        }

        private struct State
        {
            public Color Color;
            public Vector2 Scale;
            public Vector2 Position;
            public Quaternion Rotation;

            public State(Color color, Vector2 scale, Vector2 position, Quaternion rotation)
            {
                Color = color;
                Scale = scale;
                Position = position;
                Rotation = rotation;
            }
        }
    }
}