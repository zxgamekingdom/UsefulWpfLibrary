using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.LoopTasks
{
    public static class LoopTask
    {
        public static LoopInfo Create(Func<CancellationToken, Task> func,
            TimeSpan? delayTimeSpan = default,
            CancellationToken? token = default)
        {
            return new LoopInfo(func, delayTimeSpan, token);
        }

        public static LoopInfo Create(Action<CancellationToken> action,
            TimeSpan? delayTimeSpan = default,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    action.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                delayTimeSpan,
                token);
        }

        public static Task Run(Func<CancellationToken, Task> func,
            TimeSpan? delayTimeSpan = default,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(func, delayTimeSpan, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Action<CancellationToken> action,
            TimeSpan? delayTimeSpan = default,
            CancellationToken? token = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Create(action, delayTimeSpan, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }
    }
}
