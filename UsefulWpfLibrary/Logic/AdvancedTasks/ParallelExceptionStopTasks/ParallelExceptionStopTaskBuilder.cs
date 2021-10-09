using System.Threading;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelExceptionStopTasks
{
    public struct ParallelExceptionStopTaskBuilder
    {
        public ParallelExceptionStopTask Create(CancellationToken? token = null)
        {
            return new ParallelExceptionStopTask(token);
        }
    }
}
