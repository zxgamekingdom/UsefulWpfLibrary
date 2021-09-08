using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    internal static class RetryTaskIsContinue
    {
        internal static async Task<(bool isContinue, InternalRetryInfo? retryInfo)>
            IsContinue(Exception e,
                ConcurrentBag<object> bag,
                uint retriedCount,
                CancellationToken token)
        {
            var isContinue = false;
            InternalRetryInfo buff = default;
            foreach (object info in bag)
            {
                var internalRetryInfo = new InternalRetryInfo(info, retriedCount, e);
                if (internalRetryInfo.ExceptionType != e.GetType()) continue;
                buff = internalRetryInfo;
                await buff.OnExceptionThrow(token).ConfigureAwait(false);
                ContinueRetry continueRetry =
                    await buff.ContinueRetry(token).ConfigureAwait(false);
                if (continueRetry == ContinueRetry.Continue)
                {
                    isContinue = true;
                }
            }

            return (isContinue, buff);
        }
    }
}
