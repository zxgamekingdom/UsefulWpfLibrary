using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class TaskActionTask : AbstractActionTask
        {
            private readonly Func<Task> _func;

            public TaskActionTask(Func<Task> func)
            {
                _func = func;
            }

            public override Task Run()
            {
                var task = new Task<Task>(async () =>
                    {
                        try
                        {
                            await _func.Invoke();
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

    public static partial class ObserveExceptionTask
    {
    }
}
