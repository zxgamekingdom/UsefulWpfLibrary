using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record RetryInfo<TException>(
        Func<CancellationToken, RetryContext<TException>, Task<ContinueRetry>>
            ContinueRetry,
        Func<CancellationToken, RetryContext<TException>, Task>? OnExceptionThrow,
        Func<CancellationToken, RetryContext<TException>, Task>? OnRetry,
        Func<CancellationToken, RetryContext<TException>, Task<TimeSpan>>? WaitTimeSpan)
        where TException : Exception
    {
        public Type ExceptionType => typeof(TException);
        public uint RetriedCount { get; private set; }

        internal void AddRetriedCount()
        {
            RetriedCount++;
        }
    }
}