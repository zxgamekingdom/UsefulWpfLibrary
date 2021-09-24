using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static class ObserveExceptionTaskTools
    {
        public static ObserveExceptionResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new ObserveExceptionResultTask<TResult>(func, token);
        }

        public static ObserveExceptionResultTask<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    var invoke = func.Invoke(cancellationToken);
                    return Task.FromResult(invoke);
                },
                token);
        }

        public static ObserveExceptionTask Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new ObserveExceptionTask(func, token);
        }

        public static ObserveExceptionTask Create(Action<CancellationToken> action,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    action.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                token);
        }

        public static Task<TResult> Run<TResult>(Func<CancellationToken, TResult> func,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, token).Run(creationOptions, scheduler);
        }

        public static Task<TResult> Run<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, token).Run(creationOptions, scheduler);
        }

        public static Task Run(Func<CancellationToken, Task> func,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, token).Run(creationOptions, scheduler);
        }

        public static Task Run(Action<CancellationToken> action,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(action, token).Run(creationOptions, scheduler);
        }
    }
}
