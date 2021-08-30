using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        abstract class AbstractFuncTask<TResult> : IFuncTask<TResult>
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

            public abstract Task<TResult> Run();

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
        }
    }
}
