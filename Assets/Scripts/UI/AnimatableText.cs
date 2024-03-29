﻿using System.Threading;
using System.Threading.Tasks;
using Misc;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AnimatableText : MonoBehaviour
    {
        public TextMeshProUGUI Text { get; private set; }
        private Transform _transform;
        private State _defaultState;
        private AsyncExecutor _executor;

        private CancellationTokenSource _colorCTS;
        private CancellationTokenSource _positionCTS;
        private CancellationTokenSource _rotationCTS;
        private CancellationTokenSource _scaleCTS;

        private void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
            _transform = GetComponent<Transform>();
            _executor = new AsyncExecutor();
            
            _colorCTS = new CancellationTokenSource();
            _positionCTS = new CancellationTokenSource();
            _rotationCTS = new CancellationTokenSource();
            _scaleCTS = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _executor.Dispose();
            _colorCTS.Dispose();
            _positionCTS.Dispose();
            _rotationCTS.Dispose();
            _scaleCTS.Dispose();
        }

        private void Start()
        {
            _defaultState = GetCurrentState();
        }

        public async Task Kick(Color color)
        {
            State shakenState = GetCurrentState();
            shakenState.Position += Vector2.one.RotatedBy(Random.Range(0f, 359f)) * Random.Range(3f, 7f);
            _transform.position = _transform.position;
            shakenState.Rotation.z += Random.Range(-0.09f, 0.09f);
            shakenState.Scale += Vector2.one * 0.4f;
            
            await Task.WhenAll(
                LerpPosition(shakenState.Position, 0.1f),
                LerpRotation(shakenState.Rotation, 0.1f),
                LerpScale(shakenState.Scale, 0.1f),
                LerpColor(color, 0.1f));
        }
        
        public Task ReturnToDefault()
        {
            return LerpToState(_defaultState, 1f);
        }

        private async Task LerpToState(State targetState, float durationSeconds)
        {
            await Task.WhenAll(
                LerpColor(targetState.Color, durationSeconds),
                LerpPosition(targetState.Position, durationSeconds),
                LerpRotation(targetState.Rotation, durationSeconds),
                LerpScale(targetState.Scale, durationSeconds));
        }
        
        public Task LerpColor(Color target, float durationSeconds)
        {
            ResetCTS(ref _colorCTS);
            Color startColor = Text.color;
            return _executor.LerpEachFrame(durationSeconds, 
                value => Text.color = value, 
                startColor, target, 
                _colorCTS.Token);
        }

        private Task LerpPosition(Vector2 target, float durationSeconds)
        {
            ResetCTS(ref _positionCTS);
            Vector2 startPosition = _transform.localPosition;
            return _executor.LerpEachFrame(durationSeconds, 
                value => _transform.localPosition = value, 
                startPosition, target, 
                _positionCTS.Token);
        }
        
        private Task LerpRotation(Quaternion target, float durationSeconds)
        {
            ResetCTS(ref _rotationCTS);
            Quaternion startRotation = _transform.rotation;
            return _executor.LerpEachFrame(durationSeconds, 
                value => _transform.rotation = value, 
                startRotation, target, 
                _positionCTS.Token);
        }
        
        private Task LerpScale(Vector2 target, float durationSeconds)
        {
            ResetCTS(ref _scaleCTS);
            Vector2 startScale = _transform.localScale;
            return _executor.LerpEachFrame(durationSeconds, 
                value => _transform.localScale = value, 
                startScale, target, 
                _positionCTS.Token);
        }
        
        private State GetCurrentState()
        {
            return new State(
                Text.color,
                _transform.localScale,
                _transform.localPosition,
                _transform.rotation);
        }

        private void ResetCTS(ref CancellationTokenSource cts)
        {
            cts.Cancel();
            cts.Dispose();
            cts = new CancellationTokenSource();
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