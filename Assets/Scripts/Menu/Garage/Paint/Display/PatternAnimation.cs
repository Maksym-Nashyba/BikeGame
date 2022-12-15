using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Menu.Garage.Paint.Display
{
    public class PatternAnimation : IDisposable
    {
        private Task _animationTask;
        private CancellationTokenSource _cancellationTokenSource;

        private PatternAnimation(Task animationTask, CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _animationTask = animationTask;
        }

        public static PatternAnimation From(Func<CancellationToken, Task> rawTask)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task task = rawTask.Invoke(cts.Token);
            return new PatternAnimation(task, cts);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
        
        public TaskAwaiter GetAwaiter()
        {
            return _animationTask.GetAwaiter();
        }

        public void Dispose()
        {
            Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}