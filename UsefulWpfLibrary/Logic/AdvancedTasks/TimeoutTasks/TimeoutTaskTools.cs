using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.TimeoutTasks
{
    public static class TimeoutTaskTools
    {
        public static TimeoutResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default)
        {
            return new TimeoutResultTask<TResult>(func,
                timeoutTimeSpan,
                timeoutMessage,
                token);
        }

        public static TimeoutResultTask<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default)
        {
            return new TimeoutResultTask<TResult>(cancellationToken =>
                {
                    var invoke = func.Invoke(cancellationToken);
                    return Task.FromResult(invoke);
                },
                timeoutTimeSpan,
                timeoutMessage,
                token);
        }

        public static TimeoutTask Create(Func<CancellationToken, Task> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default)
        {
            return new TimeoutTask(func, timeoutTimeSpan, timeoutMessage, token);
        }

        public static TimeoutTask Create(Action<CancellationToken> action,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default)
        {
            return new TimeoutTask(cancellationToken =>
                {
                    action.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                timeoutTimeSpan,
                timeoutMessage,
                token);
        }

        public static Task<TResult> Run<TResult>(Func<CancellationToken, TResult> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, timeoutTimeSpan, timeoutMessage, token)
                .Run(creationOptions, scheduler);
        }

        public static Task<TResult> Run<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, timeoutTimeSpan, timeoutMessage, token)
                .Run(creationOptions, scheduler);
        }

        public static Task Run(Func<CancellationToken, Task> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, timeoutTimeSpan, timeoutMessage, token)
                .Run(creationOptions, scheduler);
        }

        public static Task Run(Action<CancellationToken> action,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(action, timeoutTimeSpan, timeoutMessage, token)
                .Run(creationOptions, scheduler);
        }
    }
}
