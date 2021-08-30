using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class FuncTask<T> : AbstractFuncTask<T>
        {
            private readonly Func<T> _func;

            public FuncTask(Func<T> func)
            {
                _func = func;
            }

            public override Task<T> Run()
            {
                var task = new Task<T>(() =>
                    {
                        try
                        {
                            return _func.Invoke();
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
