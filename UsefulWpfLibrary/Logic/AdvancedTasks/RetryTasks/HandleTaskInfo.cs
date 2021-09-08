using System.Threading.Tasks;
using System.Threading;
using System;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record HandleTaskInfo<TException>(CreateTaskInfo CreateTaskInfo)
        where TException : Exception
    {
        public CreateTaskInfo Retry(
            Func<CancellationToken, RetryContext<TException>, Task<ContinueRetry>>
                continueRetry,
            Func<CancellationToken, RetryContext<TException>, Task>? onExceptionThrow =
                null,
            Func<CancellationToken, RetryContext<TException>, Task>? onRetry = null,
            Func<CancellationToken, RetryContext<TException>, Task<TimeSpan>>?
                waitTimeSpan = null)
        {
            return CreateTaskInfo.Config(() =>
            {
                var info = new RetryInfo<TException>(continueRetry,
                    onExceptionThrow,
                    onRetry,
                    waitTimeSpan);
                CreateTaskInfo.AddRetryInfo(info);
            });
        }

        public CreateTaskInfo Retry(uint retryCount,
            Func<CancellationToken, RetryContext<TException>, Task>? onExceptionThrow =
                null,
            Func<CancellationToken, RetryContext<TException>, Task>? onRetry = null,
            Func<CancellationToken, RetryContext<TException>, Task<TimeSpan>>?
                waitTimeSpan = null)
        {
            return Retry((_, context) => Task.FromResult(
                    context.ExceptionRetriedCount <= retryCount ?
                        ContinueRetry.Continue :
                        ContinueRetry.Break),
                onExceptionThrow,
                onRetry,
                waitTimeSpan);
        }
    }
}
