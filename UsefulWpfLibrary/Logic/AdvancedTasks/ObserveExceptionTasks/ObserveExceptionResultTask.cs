using System;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.TasksHelpers;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public record ObserveExceptionResultTask<TResult>(
        Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token)
    {
        public Task<TResult> Run(TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            var startNew = Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        return await Func.Invoke(Token.GetToken())
                            .ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        TaskExceptionObserver.OnUnhandledTaskException(e);
                        throw;
                    }
                },
                Token.GetToken(),
                creationOptions.GetCreationOptions(),
                scheduler.GetScheduler());
            return startNew.Unwrap();
        }
    }
}