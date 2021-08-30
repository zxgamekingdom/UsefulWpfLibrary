using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class TaskFuncTask<T> : AbstractFuncTask<T>
        {
            public TaskFuncTask(Func<Task<T>> func) : base(Converter(func))
            {
            }

            private static Func<CancellationToken, Task<T>> Converter(
                Func<Task<T>> func)
            {
                return _ => func.Invoke();
            }
        }
    }
}
