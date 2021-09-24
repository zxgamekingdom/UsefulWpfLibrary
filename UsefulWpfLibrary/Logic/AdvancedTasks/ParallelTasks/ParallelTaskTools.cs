using System.Threading;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public static class ParallelTaskTools
    {
        public static ParallelResultTask<TResult> Create<TResult>(
            CancellationToken? token = null)
        {
            return new ParallelResultTask<TResult>(token);
        }

        public static ParallelTask Create(CancellationToken? token = null)
        {
            return new ParallelTask(token);
        }

        public static ParallelOneResultTask<TResult> Create<TResult>(TResult result,
            CancellationToken? token = null) where TResult : class
        {
            return new ParallelOneResultTask<TResult>(result,token);
        }
    }
}
