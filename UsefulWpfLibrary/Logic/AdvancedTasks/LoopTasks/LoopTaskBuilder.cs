using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.LoopTasks
{
    public struct LoopTaskBuilder : IEquatable<LoopTaskBuilder>
    {
        public LoopTask Create(Func<CancellationToken, Task> func,
            TimeSpan delayTimeSpan,
            CancellationToken? token = default)
        {
            return new LoopTask(func, delayTimeSpan, token);
        }

        public Task Run(Func<CancellationToken, Task> func,
            TimeSpan delayTimeSpan,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, delayTimeSpan, token).Run(creationOptions, scheduler);
        }

        public bool Equals(LoopTaskBuilder other)
        {
            return true;
        }
    }
}
