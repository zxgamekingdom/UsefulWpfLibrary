using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.TasksHelpers
{
    public static class AdvancedTaskRun
    {
        public static async Task Run(Action<CancellationToken> action,
            CancellationToken token = default)
        {
            try
            {
                await Task.Factory.StartNew(() => action.Invoke(token),
                    TaskCreationOptions.LongRunning).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                TaskExceptionObserver.OnUnhandledTaskException(e);
                throw;
            }
        }

        public static async Task<TResult> Run<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken token = default)
        {
            try
            {
                return await Task.Factory.StartNew(() => func.Invoke(token),
                    TaskCreationOptions.LongRunning).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                TaskExceptionObserver.OnUnhandledTaskException(e);
                throw;
            }
        }

        public static async Task Run(Func<CancellationToken, Task> func,
            CancellationToken token = default)
        {
            try
            {
                await Task.Factory
                    .StartNew(() => func.Invoke(token), CancellationToken.None)
                    .Unwrap()
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                TaskExceptionObserver.OnUnhandledTaskException(e);
                throw;
            }
        }

        public static async Task<TResult> Run<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken token = default)
        {
            try
            {
                return await Task.Factory.StartNew(() =>
                             func.Invoke(token),
                       CancellationToken.None)
                    .Unwrap()
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                TaskExceptionObserver.OnUnhandledTaskException(e);
                throw;
            }
        }
    }
}