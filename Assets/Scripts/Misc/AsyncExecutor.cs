using System;
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

        #region LERPs
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
        #endregion

        #region EachFrameFacades
        public Task EachFrame(float durationSeconds, Action<float> action)
        {
            return EachFrame(durationSeconds, action, EaseFunctions.Lerp, CancellationToken.None);
        }
        
        public Task EachFrame(float durationSeconds, Action<float> action, EaseFunctions.Delegate easeFunction)
        {
            return EachFrame(durationSeconds, action, easeFunction, CancellationToken.None);
        }
        
        public Task EachFrame(float durationSeconds, Action<float> action, CancellationToken specialCancellationToken)
        {
            return EachFrame(durationSeconds, action, EaseFunctions.Lerp, specialCancellationToken);
        }
        #endregion
        
        public async Task EachFrame(float durationSeconds, Action<float> action, EaseFunctions.Delegate easeFunction, CancellationToken specialCancellationToken)
        {
            float timeElapsed = 0f;
            while (timeElapsed < durationSeconds)
            {
                if (_globalCancellationTokenSource.Token.IsCancellationRequested
                    || specialCancellationToken.IsCancellationRequested) return;
                action.Invoke(easeFunction.Invoke(timeElapsed/durationSeconds));
                await Task.Yield();
                timeElapsed += Time.deltaTime;
            }
        }

        public void CancelAll()
        {
            _globalCancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            CancelAll();
            _globalCancellationTokenSource?.Dispose();
        }
    }
}