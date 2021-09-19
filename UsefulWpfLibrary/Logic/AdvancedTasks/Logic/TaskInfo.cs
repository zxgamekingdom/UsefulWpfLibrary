using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.Logic
{
    public record TaskInfo<TChildTaskInfo>(CancellationToken? Token = default)
        where TChildTaskInfo : TaskInfo<TChildTaskInfo>
    {
        private TaskCreationOptions? _creationOptions;
        private TaskScheduler? _scheduler;
        public CheckTaskState TaskState { get; } = new();

        public TaskCreationOptions GetCreationOptions()
        {
            return _creationOptions ?? TaskCreationOptions.None;
        }

        public TaskScheduler GetScheduler()
        {
            return _scheduler ?? TaskScheduler.Current;
        }

        public TChildTaskInfo Config(Action action)
        {
            TaskState.Config(action);
            return (TChildTaskInfo)this;
        }

        public TChildTaskInfo SetScheduler(TaskScheduler? scheduler)
        {
            return Config(() => _scheduler = scheduler);
        }

        public TChildTaskInfo SetCreationOptions(TaskCreationOptions? creationOptions)
        {
            return Config(() => _creationOptions = creationOptions);
        }

        public CancellationToken GetToken()
        {
            return Token ?? CancellationToken.None;
        }
    }
}