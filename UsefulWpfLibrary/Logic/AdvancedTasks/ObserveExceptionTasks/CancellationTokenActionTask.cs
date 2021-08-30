using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class CancellationTokenActionTask : AbstractActionTask
        {
            private readonly Action<CancellationToken> _action;

            public CancellationTokenActionTask(Action<CancellationToken> action)
            {
                _action = action;
            }

            public override Task Run()
            {
                var task = new Task(() =>
                    {
                        try
                        {
                            _action.Invoke(GetCancellationToken());
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
                return task;
            }
        }
    }
}
