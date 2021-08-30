using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class ActionTask : AbstractActionTask
        {
            private readonly Action _action;

            public ActionTask(Action action)
            {
                _action = action;
            }

            public override Task Run()
            {
                var task = new Task(() =>
                    {
                        try
                        {
                            _action.Invoke();
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
