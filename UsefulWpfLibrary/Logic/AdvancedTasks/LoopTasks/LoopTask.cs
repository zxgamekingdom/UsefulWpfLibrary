using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.LoopTasks
{
    public record LoopTask(Func<CancellationToken, Task> Func,
        TimeSpan DelayTimeSpan,
        CancellationToken? Token)
    {
        public Task Run(TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            var token = Token.GetToken();
            return Task.Factory.StartNew(async () =>
                    {
                        while (token.IsCancellationRequested is false)
                        {
                            await Func.Invoke(token).ConfigureAwait(false);
                            await Task.Delay(DelayTimeSpan, token)
                                .ConfigureAwait(false);
                        }
                    },
                    token,
                    creationOptions.GetCreationOptions(),
                    scheduler.GetScheduler())
                .Unwrap();
        }
    }
}
