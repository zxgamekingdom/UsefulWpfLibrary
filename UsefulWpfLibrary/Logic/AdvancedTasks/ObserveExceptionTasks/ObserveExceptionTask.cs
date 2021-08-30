using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        public static IActionTask Create(Action action)
        {
            return new ActionTask(action);
        }

        public static IActionTask Create(Action<CancellationToken> action)
        {
            return new CancellationTokenActionTask(action);
        }

        public static IActionTask Create(Func<Task> func)
        {
            return new TaskActionTask(func);
        }

        public static IActionTask Create(Func<CancellationToken, Task> func)
        {
            return new CancellationTokenTaskActionTask(func);
        }

        public static IFuncTask<TResult> Create<TResult>(Func<TResult> func)
        {
            return new FuncTask<TResult>(func);
        }

        public static IFuncTask<TResult> Create<TResult>(Func<Task<TResult>> func)
        {
            return new TaskFuncTask<TResult>(func);
        }

        public static IFuncTask<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func)
        {
            return new CancellationTokenFuncTask<TResult>(func);
        }

        public static IFuncTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func)
        {
            return new CancellationTokenTaskFuncTask<TResult>(func);
        }

        public static Task Run(Action action,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(action).SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Action<CancellationToken> action,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(action)
                .SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Func<Task> func,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(func).SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static IActionTask Run(Func<CancellationToken, Task> func,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(func)
                .SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler);
        }

        public static Task<TResult> Run<TResult>(Func<TResult> func,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(func).SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static IFuncTask<TResult> Run<TResult>(Func<Task<TResult>> func,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(func)
                .SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler);
        }

        public static IFuncTask<TResult> Run<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(func)
                .SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler);
        }

        public static IFuncTask<TResult> Run<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? cancellationToken = default,
            TaskCreationOptions? creationOptions = TaskCreationOptions.None,
            TaskScheduler? scheduler = null)
        {
            return Create(func)
                .SetCancellationToken(cancellationToken)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler);
        }
    }
}
