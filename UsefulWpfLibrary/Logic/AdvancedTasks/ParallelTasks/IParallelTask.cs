using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public static partial class ParallelTask
    {
        public interface IParallelTask
        {
            IParallelTask SetCancellationToken(CancellationToken? cancellationToken);

            IParallelTask AddTask(Func<CancellationToken, Task> func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null);

            IParallelTask AddTask(Action<CancellationToken> func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null);

            IParallelTask AddTask(Action func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null);

            IParallelTask AddTask(Func<Task> func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null);

            Task Run();
        }
    }
}
