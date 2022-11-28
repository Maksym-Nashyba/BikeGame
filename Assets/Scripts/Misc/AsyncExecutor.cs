﻿using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Misc
{
    public class AsyncExecutor : IDisposable
    {
        private CancellationTokenSource _globalCancellationTokenSource;

        public AsyncExecutor()
        {
            _globalCancellationTokenSource = new CancellationTokenSource();
        }

        public Task LerpEachFrame(float durationSeconds, Action<float> valueSetter, float from, float to, CancellationToken cancellationToken)
        {
            return EachFrame(durationSeconds, t => valueSetter(Mathf.Lerp(from, to, t)), cancellationToken);
        }
        
        public Task LerpEachFrame(float durationSeconds, Action<Color> valueSetter, Color from, Color to, CancellationToken cancellationToken)
        {
            return EachFrame(durationSeconds, t => valueSetter(Color.Lerp(from, to, t)), cancellationToken);
        }
        
        public Task LerpEachFrame(float durationSeconds, Action<Vector2> valueSetter, Vector2 from, Vector2 to, CancellationToken cancellationToken)
        {
            return EachFrame(durationSeconds, t => valueSetter(Vector2.Lerp(from, to, t)), cancellationToken);
        }
        
        public Task LerpEachFrame(float durationSeconds, Action<Quaternion> valueSetter, Quaternion from, Quaternion to, CancellationToken cancellationToken)
        {
            return EachFrame(durationSeconds, t => valueSetter(Quaternion.Lerp(from, to, t)), cancellationToken);
        }
        
        public Task EachFrame(float durationSeconds, Action<float> action)
        {
            return EachFrame(durationSeconds, action, CancellationToken.None);
        }
        
        public async Task EachFrame(float durationSeconds, Action<float> action, CancellationToken specialCancellationToken)
        {
            float timeElapsed = 0f;
            while (timeElapsed < durationSeconds)
            {
                if (_globalCancellationTokenSource.Token.IsCancellationRequested
                    || specialCancellationToken.IsCancellationRequested) return;
                await Task.Yield();
                action.Invoke(timeElapsed/durationSeconds);
                timeElapsed += Time.deltaTime;
            }
        }

        public void CancelAll()
        {
            _globalCancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            _globalCancellationTokenSource?.Dispose();
        }
    }
}