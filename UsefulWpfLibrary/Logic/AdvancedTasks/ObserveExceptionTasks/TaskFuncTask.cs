using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class TaskFuncTask<T> : AbstractFuncTask<T>
        {
            private readonly Func<Task<T>> _func;

            public TaskFuncTask(Func<Task<T>> func)
            {
                _func = func;
            }

            public override Task<T> Run()
            {
                var task = new Task<Task<T>>(async () =>
                    {
                        try
                        {
                            return await _func.Invoke();
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
