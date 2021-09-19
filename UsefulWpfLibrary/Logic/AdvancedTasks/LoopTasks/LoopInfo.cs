using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.LoopTasks
{
    public record LoopInfo(Func<CancellationToken, Task> Func,
        TimeSpan? DelayTimeSpan = null,
        CancellationToken? Token = default) : TaskInfo<LoopInfo>(Token)
    {
        public Task Run()
        {
            TaskState.Start();
            var task = Task.Factory.StartNew(() =>
                {
                    return TaskState.Run(async () =>
                    {
                        Token?.ThrowIfCancellationRequested();
                        try
                        {
                            while (true)
                            {
                                await Func.Invoke(GetToken()).ConfigureAwait(false);
                                await Task.Delay(Delay).ConfigureAwait(false);
                            }
                        }
                        catch (Exception e)
                        {
                            TaskExceptionObserver.OnUnhandledTaskException(e);
                            throw;
                        }
                    });
                },
                CancellationToken.None,
                GetCreationOptions(),
                GetScheduler());
            return task.Unwrap();
        }

        public int Delay =>
            DelayTimeSpan switch
            {
                { } value => (int)value.TotalMilliseconds,
                _ => 1,
            };
    }
}
