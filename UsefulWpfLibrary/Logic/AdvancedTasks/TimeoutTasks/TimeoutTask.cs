using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.Extensions;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.TimeoutTasks
{
    public record TimeoutTask(Func<CancellationToken, Task> Func,
        TimeSpan TimeoutTimeSpan,
        string? TimeoutMessage,
        CancellationToken? Token)
    {
        public Task Run(TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            var timeoutCts = new CancellationTokenSource(TimeoutTimeSpan);
            var token = Token.GetToken()
                .Link(new[] { timeoutCts.Token })
                .token;
            var startNew = Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        await Func.Invoke(token).ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        if (e is TaskCanceledException or OperationCanceledException &&
                            timeoutCts.IsCancellationRequested)
                        {
                            throw new TimeoutException(TimeoutMessage ?? "动作执行超时", e);
                        }

                        throw;
                    }
                    finally
                    {
                        timeoutCts.Dispose();
                    }
                },
                token,
                creationOptions.GetCreationOptions(),
                scheduler.GetScheduler());
            return startNew.Unwrap();
        }
    }
}
