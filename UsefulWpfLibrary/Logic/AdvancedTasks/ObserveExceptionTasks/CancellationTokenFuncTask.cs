using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        class CancellationTokenFuncTask<T> : AbstractFuncTask<T>
        {
            private readonly Func<CancellationToken, T> _func;

            public CancellationTokenFuncTask(Func<CancellationToken, T> func)
            {
                _func = func;
            }

            public override Task<T> Run()
            {
                var task = new Task<T>(() =>
                    {
                        try
                        {
                            return _func.Invoke(GetCancellationToken());
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
