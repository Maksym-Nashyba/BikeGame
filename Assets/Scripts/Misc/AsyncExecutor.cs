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

        public async Task EachFrame(float durationSeconds, Action<float> action)
        {
            float timeElapsed = 0f;
            while (timeElapsed < durationSeconds)
            {
                if (_globalCancellationTokenSource.Token.IsCancellationRequested) return;
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