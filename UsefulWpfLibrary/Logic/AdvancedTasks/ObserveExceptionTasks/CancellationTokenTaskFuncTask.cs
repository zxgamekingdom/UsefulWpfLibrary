using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class CancellationTokenTaskFuncTask<T> : AbstractFuncTask<T>
        {
            private readonly Func<CancellationToken, Task<T>> _func;

            public CancellationTokenTaskFuncTask(Func<CancellationToken, Task<T>> func)
            {
                _func = func;
            }

            public override Task<T> Run()
            {
                var task = new Task<Task<T>>(async () =>
                    {
                        try
                        {
                            return await _func.Invoke(GetCancellationToken());
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
