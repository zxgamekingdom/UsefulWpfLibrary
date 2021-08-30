using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class CancellationTokenTaskActionTask : AbstractActionTask
        {
            private readonly Func<CancellationToken, Task> _func;

            public CancellationTokenTaskActionTask(Func<CancellationToken, Task> func)
            {
                _func = func;
            }

            public override Task Run()
            {
                var task = new Task<Task>(async () =>
                    {
                        try
                        {
                            await _func.Invoke(GetCancellationToken());
                        }
                        catch (Exception e)
                        {
                            TaskExceptionObserver.OnUnhandledTaskException(e);
                            throw;
                        }
                    },
                    GetCancellationToken(),
                    GetCreationOptions());
                task.Start(GetScheduler());
                return task.Unwrap();
            }
        }
    }
}
