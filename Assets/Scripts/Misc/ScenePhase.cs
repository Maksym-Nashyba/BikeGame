using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace Misc
{
    public class ScenePhase
    {
        public CompletionStage Stage { get; private set; }

        private List<Func<Task>> _awaitedTasks;
        private List<Func<Task>> _fireAndForgetTasks;

        public ScenePhase()
        {
            Stage = CompletionStage.NotStarted;
            _awaitedTasks = new List<Func<Task>>();
            _fireAndForgetTasks = new List<Func<Task>>();
        }

        public TaskAwaiter GetAwaiter()
        {
            if (Stage > CompletionStage.NotStarted) throw new InvalidOperationException("Scene phase can only be started once");
            Stage = CompletionStage.Running;
            
            foreach (Func<Task> fireAndForgetTask in _fireAndForgetTasks)
            {
                fireAndForgetTask.Invoke();
            }
            Task[] awaitedTasks = new Task[_awaitedTasks.Count];
            for (int i = 0; i < awaitedTasks.Length; i++)
            {
                awaitedTasks[i] = _awaitedTasks[i].Invoke();
            }

            Func<Task> combinedTask = async () =>
            {
                await Task.WhenAll(awaitedTasks);
                Stage = CompletionStage.Completed;
                Debug.Log("I came");
            };
            return combinedTask().GetAwaiter();
        }

        public void SubscribeAwaited(Func<Task> task)
        {
            _awaitedTasks.Add(task);
        }
        
        public void SubscribeFireAndForget(Func<Task> task)
        {
            _fireAndForgetTasks.Add(task);
        }

        public enum CompletionStage
        {
            NotStarted,
            Running,
            Completed
        }
    }
}