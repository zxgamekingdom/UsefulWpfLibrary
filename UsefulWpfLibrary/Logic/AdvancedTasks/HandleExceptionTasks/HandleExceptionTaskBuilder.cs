using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public struct HandleExceptionTaskBuilder
    {
        public HandleExceptionResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new HandleExceptionResultTask<TResult>(func, token);
        }

        public HandleExceptionTask Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new HandleExceptionTask(func, token);
        }
    }
}
