using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.TimeoutTasks
{
    public struct TimeoutTaskBuilder
    {
        public TimeoutResultTask<TResult> Create<TResult>(
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

        public TimeoutTask Create(Func<CancellationToken, Task> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default)
        {
            return new TimeoutTask(func, timeoutTimeSpan, timeoutMessage, token);
        }

        public Task<TResult> Run<TResult>(Func<CancellationToken, Task<TResult>> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, timeoutTimeSpan, timeoutMessage, token)
                .Run(creationOptions, scheduler);
        }

        public Task Run(Func<CancellationToken, Task> func,
            TimeSpan timeoutTimeSpan,
            string? timeoutMessage = null,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, timeoutTimeSpan, timeoutMessage, token)
                .Run(creationOptions, scheduler);
        }
    }
}
