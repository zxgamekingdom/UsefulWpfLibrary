using System.Threading;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public static class ParallelTask
    {
        public static ParallelInfo Create(CancellationToken? token = default)
        {
            return new ParallelInfo(token);
        }

        public static CollectionResultParallelInfo<TResult> Create<TResult>(
            CancellationToken? token = default)
        {
            return new CollectionResultParallelInfo<TResult>(token);
        }

        public static SingleResultParallelInfo<TResult> Create<TResult>(TResult para,
            CancellationToken? token = default) where TResult : class

        {
            return new SingleResultParallelInfo<TResult>(para, token);
        }
    }
}