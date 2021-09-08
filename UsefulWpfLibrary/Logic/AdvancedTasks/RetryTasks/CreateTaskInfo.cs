using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record CreateTaskInfo(Func<CancellationToken, Task> Func,
        CancellationToken? Token) : TaskInfo<CreateTaskInfo>(Token)
    {
        private readonly ConcurrentBag<object> _bag = new();
        public uint RetriedCount { get; private set; }

        public HandleTaskInfo<TException> HandleException<TException>()
            where TException : Exception
        {
            HandleTaskInfo<TException>? buff = null;
            _ = Config(() => buff = new HandleTaskInfo<TException>(this));
            return buff!;
        }

        public Task Run()
        {
            TaskState.Start();
            return TaskState.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        await ObserveExceptionTask.Run(Func,
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
