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

        public TChildTaskInfo SetScheduler(TaskScheduler? scheduler)
        {
            TaskState.CheckNotStarted();
            _scheduler = scheduler;
            return (TChildTaskInfo)this;
        }

        public TChildTaskInfo SetCreationOptions(TaskCreationOptions? creationOptions)
        {
            TaskState.CheckNotStarted();
            _creationOptions = creationOptions;
            return (TChildTaskInfo)this;
        }

        public CancellationToken GetToken()
        {
            return Token ?? CancellationToken.None;
        }
    }
}
