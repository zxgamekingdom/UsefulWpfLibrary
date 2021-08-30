using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private abstract class AbstractActionTask : IActionTask
        {
            private CancellationToken? _cancellationToken;
            private TaskCreationOptions? _creationOptions;
            private TaskScheduler? _scheduler;

            protected CancellationToken GetCancellationToken()
            {
                return _cancellationToken ?? CancellationToken.None;
            }

            protected TaskCreationOptions GetCreationOptions()
            {
                return _creationOptions ?? TaskCreationOptions.None;
            }

            protected TaskScheduler GetScheduler()
            {
                return _scheduler ?? TaskScheduler.Current;
            }

            public abstract Task Run();

            public IActionTask SetCancellationToken(
                CancellationToken? cancellationToken)
            {
                _cancellationToken = cancellationToken;
                return this;
            }

            public IActionTask SetCreationOptions(TaskCreationOptions? creationOptions)
            {
                _creationOptions = creationOptions;
                return this;
            }

            public IActionTask SetScheduler(TaskScheduler? scheduler)
            {
                _scheduler = scheduler;
                return this;
            }
        }
    }
}
