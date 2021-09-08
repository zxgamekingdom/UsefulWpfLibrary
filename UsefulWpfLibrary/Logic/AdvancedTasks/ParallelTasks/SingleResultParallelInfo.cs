using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public record SingleResultParallelInfo<TResult>(TResult Result,
        CancellationToken? Token = default) :
        TaskInfo<SingleResultParallelInfo<TResult>>(Token) where TResult : class
    {
        private readonly ConcurrentBag<ActionInfo> _bag = new();

        public Task<TResult> Run()
        {
            TaskState.Start();
            return TaskState.Run(async () =>
            {
                Token?.ThrowIfCancellationRequested();
                ActionInfo[] funcInfos = _bag.ToArray();
                int length = funcInfos.Length;
                var tasks = new Task[length];
                for (var i = 0; i < length; i++)
                {
                    tasks[i] = funcInfos[i].Run();
                }

                try
                {
                    await Task.WhenAll(tasks).ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }

                List<Exception> exceptions = new(length);
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var index = 0; index < tasks.Length; index++)
                {
                    var task = tasks[index];
                    try
                    {
                        await task.ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }

                return exceptions.Count != 0 ?
                    throw new AggregateException(exceptions) :
                    Result;
            });
        }

        public SingleResultParallelInfo<TResult> AddTask(
            Func<TResult, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            return Config(() =>
            {
                _bag.Add(ObserveExceptionTask
                    .Create(async token =>
                            await func.Invoke(Result, token).ConfigureAwait(false),
                        GetToken())
                    .SetCreationOptions(creationOptions)
                    .SetScheduler(scheduler));
            });
        }

        public SingleResultParallelInfo<TResult> AddTask(
            Action<TResult, CancellationToken> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            _ = AddTask((result, token) =>
                {
                    func.Invoke(result, token);
                    return Task.CompletedTask;
                }, creationOptions, scheduler);
            return this;
        }
    }
}