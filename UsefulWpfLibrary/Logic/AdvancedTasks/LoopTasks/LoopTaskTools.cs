using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.LoopTasks
{
    public static class LoopTaskTools
    {
        public static LoopTask Create(Func<CancellationToken, Task> func,
            TimeSpan delayTimeSpan,
            CancellationToken? token = default)
        {
            return new LoopTask(func, delayTimeSpan, token);
        }

        public static LoopTask Create(Action<CancellationToken> func,
            TimeSpan delayTimeSpan,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    func.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                delayTimeSpan,
                token);
        }

        public static Task Run(Func<CancellationToken, Task> func,
            TimeSpan delayTimeSpan,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, delayTimeSpan, token).Run(creationOptions, scheduler);
        }

        public static Task Run(Action<CancellationToken> func,
            TimeSpan delayTimeSpan,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, delayTimeSpan, token).Run(creationOptions, scheduler);
        }
    }
}
