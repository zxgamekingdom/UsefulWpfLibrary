using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class CancellationTokenFuncTask<T> : AbstractFuncTask<T>
        {
            public CancellationTokenFuncTask(Func<CancellationToken, T> func) : base(
                Converter(func))
            {
            }

            private static Func<CancellationToken, Task<T>> Converter(
                Func<CancellationToken, T> func)
            {
                return token => Task.FromResult(func.Invoke(token));
            }
        }
    }
}
