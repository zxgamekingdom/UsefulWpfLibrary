using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public static class HandleExceptionTaskTools
    {
        public static HandleExceptionResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new HandleExceptionResultTask<TResult>(func, token);
        }

        public static HandleExceptionResultTask<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                    Task.FromResult(func.Invoke(cancellationToken)),
                token);
        }

        public static HandleExceptionTask Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new HandleExceptionTask(func, token);
        }

        public static HandleExceptionTask Create(Action<CancellationToken> action,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    action.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                token);
        }
    }
}
