using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public record FuncInfo<TResult>(Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token = default)
    {
        private TaskCreationOptions? _creationOptions;
        private TaskScheduler? _scheduler;
        public CheckTaskState TaskState { get; } = new();

        public FuncInfo<TResult> SetScheduler(TaskScheduler? scheduler)
        {
            TaskState.CheckState();
            if (scheduler != null) _scheduler = scheduler;
            return this;
        }

        public FuncInfo<TResult> SetCreationOptions(
            TaskCreationOptions? creationOptions)
        {
            TaskState.CheckState();
            if (creationOptions != null) _creationOptions = creationOptions;
            return this;
        }

        public Task<TResult> Run()
        {
            TaskState.Start();
            var task = Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        return await Func.Invoke(GetToken());
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
    }
}
