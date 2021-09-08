using System;
using System.Threading;
using System.Threading.Tasks;

using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public record FuncInfo<TResult>(Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token = default) : TaskInfo<FuncInfo<TResult>>(Token)
    {
        public Task<TResult> Run()
        {
            TaskState.Start();
            var task = Task.Factory.StartNew(() =>
                {
                    return TaskState.Run(async () =>
                    {
                        Token?.ThrowIfCancellationRequested();
                        try
                        {
                            return await Func.Invoke(GetToken()).ConfigureAwait(false);
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