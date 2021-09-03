using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public static class HandleExceptionTask
    {
        public static HandleReturnResultExceptionInfo<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new HandleReturnResultExceptionInfo<TResult>(func, token);
        }

        public static HandleReturnResultExceptionInfo<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                    Task.FromResult(func.Invoke(cancellationToken)),
                token);
        }

        public static HandleExceptionInfo Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new HandleExceptionInfo(func, token);
        }

        public static HandleExceptionInfo Create(Action<CancellationToken> func,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
            {
                func.Invoke(cancellationToken);
                return Task.CompletedTask;
            });
        }
    }
}
