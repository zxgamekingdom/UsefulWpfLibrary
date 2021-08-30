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

            void CheckState()
            {
                if (IsStarted)
                {
                    throw new InvalidOperationException("任务已经开始运行了,无法执行此操作");
                }
            }

            public bool IsStarted { get; private set; }

            public IFuncTask<TResult> SetCancellationToken(
                CancellationToken? cancellationToken)
            {
                CheckState();
                _cancellationToken = cancellationToken;
                return this;
            }

            public IFuncTask<TResult> SetCreationOptions(
                TaskCreationOptions? creationOptions)
            {
                CheckState();
                _creationOptions = creationOptions;
                return this;
            }

            public IFuncTask<TResult> SetScheduler(TaskScheduler? scheduler)
            {
                CheckState();
                _scheduler = scheduler;
                return this;
            }

            public Task<TResult> Run()
            {
                CheckState();
                IsStarted = true;
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
