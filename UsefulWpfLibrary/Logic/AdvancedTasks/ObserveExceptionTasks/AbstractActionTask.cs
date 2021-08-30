using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private abstract class AbstractActionTask : IActionTask
        {
            private CancellationToken? _cancellationToken;
            private TaskCreationOptions? _creationOptions;
            private TaskScheduler? _scheduler;
            private readonly Func<CancellationToken, Task> _func;

            protected AbstractActionTask(Func<CancellationToken, Task> func)
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

            public IActionTask SetCancellationToken(
                CancellationToken? cancellationToken)
            {
                TaskState.CheckState();
                _cancellationToken = cancellationToken;
                return this;
            }

            public IActionTask SetCreationOptions(TaskCreationOptions? creationOptions)
            {
                TaskState.CheckState();
                _creationOptions = creationOptions;
                return this;
            }

            public IActionTask SetScheduler(TaskScheduler? scheduler)
            {
                TaskState.CheckState();
                _scheduler = scheduler;
                return this;
            }

            public CheckTaskState TaskState { get; } = new();

            public Task Run()
            {
                TaskState.Start();
                var task = new Task<Task>(async () =>
                    {
                        try
                        {
                            await _func.Invoke(GetCancellationToken());
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
