using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class TaskActionTask : AbstractActionTask
        {
            public TaskActionTask(Func<Task> func) : base(Converter(func))
            {
            }

            private static Func<CancellationToken, Task> Converter(Func<Task> func)
            {
                return _ => func.Invoke();
            }
        }
    }
}
