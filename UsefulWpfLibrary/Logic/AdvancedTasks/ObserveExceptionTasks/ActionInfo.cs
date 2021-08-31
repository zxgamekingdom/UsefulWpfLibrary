using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public record ActionInfo(Func<CancellationToken, Task> Action,
        CancellationToken? Token = default)
    {
        private TaskCreationOptions? _creationOptions;
        private TaskScheduler? _scheduler;
        public CheckTaskState TaskState { get; } = new();

        public ActionInfo SetScheduler(TaskScheduler? scheduler)
        {
            TaskState.CheckState();
            if (scheduler != null) _scheduler = scheduler;
            return this;
        }

        public Task Run()
        {
            TaskState.Start();
            var task = Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Action.Invoke(GetToken());
                    }
                    catch (Exception e)
                    {
                        TaskExceptionObserver.OnUnhandledTaskException(e);
                        throw;
                    }
                },
                CancellationToken.None,
                GetCreationOptions(),
                GetScheduler());
            return task.Unwrap();
        }

        private CancellationToken GetToken()
        {
            return Token ?? CancellationToken.None;
        }

        private TaskCreationOptions GetCreationOptions()
        {
            return _creationOptions ?? TaskCreationOptions.None;
        }

        private TaskScheduler GetScheduler()
        {
            return _scheduler ?? TaskScheduler.Current;
        }

        public ActionInfo SetCreationOptions(TaskCreationOptions? creationOptions)
        {
            TaskState.CheckState();
            if (creationOptions != null) _creationOptions = creationOptions;
            return this;
        }
    }
}
