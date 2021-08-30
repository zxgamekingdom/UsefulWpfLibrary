using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class FuncTask<T> : AbstractFuncTask<T>
        {
            public FuncTask(Func<T> func) : base(Converter(func))
            {
            }

            private static Func<CancellationToken, Task<T>> Converter(Func<T> func)
            {
                return _ => Task.FromResult(func.Invoke());
            }
        }
    }
}
