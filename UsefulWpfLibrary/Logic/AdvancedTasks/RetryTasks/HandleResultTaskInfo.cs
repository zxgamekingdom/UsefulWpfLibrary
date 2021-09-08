using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record HandleResultTaskInfo<TResult, TException>(
        CreateResultTaskInfo<TResult> CreateResultTaskInfo) where TException : Exception
    {
        public CreateResultTaskInfo<TResult> Retry(
            Func<CancellationToken, RetryContext<TException>, Task<ContinueRetry>>
                continueRetry,
            Func<CancellationToken, RetryContext<TException>, Task>? onExceptionThrow =
                null,
            Func<CancellationToken, RetryContext<TException>, Task>? onRetry = null,
            Func<CancellationToken, RetryContext<TException>, Task<TimeSpan>>?
                waitTimeSpan = null)
        {
            return CreateResultTaskInfo.Config(() =>
            {
                var info = new RetryInfo<TException>(continueRetry,
                    onExceptionThrow,
                    onRetry,
                    waitTimeSpan);
                CreateResultTaskInfo.AddRetryInfo(info);
            });
        }

        public CreateResultTaskInfo<TResult> Retry(uint retryCount,
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