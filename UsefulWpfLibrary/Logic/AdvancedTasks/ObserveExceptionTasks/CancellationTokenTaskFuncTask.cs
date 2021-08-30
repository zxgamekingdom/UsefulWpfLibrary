using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class CancellationTokenTaskFuncTask<T> : AbstractFuncTask<T>
        {
            public CancellationTokenTaskFuncTask(Func<CancellationToken, Task<T>> func)
                : base(func)
            {
            }
        }
    }
}
