using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        abstract class AbstractFuncTask<TResult> : IFuncTask<TResult>
        {
            private CancellationToken? _cancellationToken;
            private TaskCreationOptions? _creationOptions;
            private TaskScheduler? _scheduler;
            private readonly Func<CancellationToken, Task<TResult>> _func;

            protected AbstractFuncTask(Func<CancellationToken, Task<TResult>> func)
            {
                _func = func;
            }

            private CancellationToken GetCancellationToken()
            {
                return _cancellationToken ?? CancellationToken.None;
            }

            private TaskCreationOptions GetCreationOptions()
            {
                return _creationOptions ?? TaskCreationOptions.None;
            }

            private TaskScheduler GetScheduler()
            {
                return _scheduler ?? TaskScheduler.Current;
            }

            public IFuncTask<TResult> SetCancellationToken(
                CancellationToken? cancellationToken)
            {
                _cancellationToken = cancellationToken;
                return this;
            }

            public IFuncTask<TResult> SetCreationOptions(
                TaskCreationOptions? creationOptions)
            {
                _creationOptions = creationOptions;
                return this;
            }

            public IFuncTask<TResult> SetScheduler(TaskScheduler? scheduler)
            {
                _scheduler = scheduler;
                return this;
            }

            public Task<TResult> Run()
            {
                var task = new Task<Task<TResult>>(async () =>
                    {
                        try
                        {
                            return await _func.Invoke(GetCancellationToken());
                        }
                        catch (Exception e)
                        {
                            TaskExceptionObserver.OnUnhandledTaskException(e);
                            throw;
                        }
                    },
                    GetCancellationToken(),
                    GetCreationOptions());
                task.Start(GetScheduler());
                return task.Unwrap();
            }
        }
    }
}
