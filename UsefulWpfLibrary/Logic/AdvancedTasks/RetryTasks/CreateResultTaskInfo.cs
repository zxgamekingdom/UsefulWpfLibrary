using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record CreateResultTaskInfo<TResult>(
        Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token) : TaskInfo<CreateResultTaskInfo<TResult>>(Token)
    {
        private readonly ConcurrentBag<object> _bag = new();
        public uint RetriedCount { get; private set; }

        public HandleResultTaskInfo<TResult, TException> HandleException<TException>()
            where TException : Exception
        {
            HandleResultTaskInfo<TResult, TException>? buff = null;
            _ = Config(() =>
                buff = new HandleResultTaskInfo<TResult, TException>(this));
            return buff!;
        }

        public Task<TResult> Run()
        {
            TaskState.Start();
            return TaskState.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        return await ObserveExceptionTask.Run(Func,
                                GetCreationOptions(),
                                GetScheduler(),
                                GetToken())
                            .ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        (bool isContinue, InternalRetryInfo? retryInfo) =
                            await RetryTaskIsContinue
                                .IsContinue(e, _bag, RetriedCount, GetToken())
                                .ConfigureAwait(false);
                        if (isContinue is false) throw;
                        await retryInfo!.Value.OnRetry(GetToken())
                            .ConfigureAwait(false);
                        await retryInfo.Value.WaitTimeSpan(GetToken())
                            .ConfigureAwait(false);
                        AddRetriedCount();
                        retryInfo.Value.AddRetriedCount();
                    }
                }
            });
        }

        internal void AddRetriedCount()
        {
            RetriedCount++;
        }

        public void AddRetryInfo<TException>(RetryInfo<TException> retryInfo)
            where TException : Exception
        {
            _bag.Add(retryInfo);
        }
    }
}
