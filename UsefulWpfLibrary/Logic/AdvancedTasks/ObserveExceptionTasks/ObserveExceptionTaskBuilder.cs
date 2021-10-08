using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public struct ObserveExceptionTaskBuilder
    {
        public ObserveExceptionResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new ObserveExceptionResultTask<TResult>(func, token);
        }

        public ObserveExceptionTask Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new ObserveExceptionTask(func, token);
        }

        public Task<TResult> Run<TResult>(Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, token).Run(creationOptions, scheduler);
        }

        public Task Run(Func<CancellationToken, Task> func,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, token).Run(creationOptions, scheduler);
        }
    }
}
