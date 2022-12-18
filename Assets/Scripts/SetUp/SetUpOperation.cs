using System;
using System.Threading.Tasks;

namespace SetUp
{
    public class SetUpOperation
    {
        public readonly Func<Task> Task;
        public readonly string DoneMessage;
        public readonly bool IsCritical;

        public SetUpOperation(Func<Task> task, string doneMessage, bool isCritical)
        {
            Task = task;
            DoneMessage = doneMessage;
            IsCritical = isCritical;
        }
    }
}