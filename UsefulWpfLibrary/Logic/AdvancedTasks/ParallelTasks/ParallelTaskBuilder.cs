using System.Threading;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public struct ParallelTaskBuilder
    {
        public ParallelResultTask<TResult> Create<TResult>(CancellationToken? token =
            null)
        {
            return new ParallelResultTask<TResult>(token);
        }

        public ParallelTask Create(CancellationToken? token = null)
        {
            return new ParallelTask(token);
        }

        public ParallelOneResultTask<TResult> Create<TResult>(TResult result,
            CancellationToken? token = null) where TResult : class
        {
            return new ParallelOneResultTask<TResult>(result, token);
        }
    }
}
