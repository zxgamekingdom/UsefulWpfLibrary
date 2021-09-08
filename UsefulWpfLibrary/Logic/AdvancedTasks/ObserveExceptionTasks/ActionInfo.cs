using System;
using System.Threading;
using System.Threading.Tasks;

using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public record ActionInfo(Func<CancellationToken, Task> Action,
        CancellationToken? Token = default) : TaskInfo<ActionInfo>(Token)
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
                            await Action.Invoke(GetToken()).ConfigureAwait(false);
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
    }
}