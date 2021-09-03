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
        CancellationToken? Token = default) where TResult : class
    {
        private readonly ConcurrentBag<ActionInfo> _bag = new();
        public CheckTaskState TaskState { get; } = new();

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

                if (exceptions.Count != 0)
                {
                    throw new AggregateException(exceptions);
                }

                return Result;
            });
        }

        CancellationToken GetToken()
        {
            return Token ?? CancellationToken.None;
        }

        public SingleResultParallelInfo<TResult> AddTask(
            Func<TResult, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            TaskState.CheckNotStarted();
            _bag.Add(ObserveExceptionTask
                .Create(async _ => await func.Invoke(Result, GetToken()), Token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler));
            return this;
        }

        public SingleResultParallelInfo<TResult> AddTask(
            Action<TResult, CancellationToken> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            AddTask((result, token) =>
            {
                func.Invoke(result, token);
                return Task.CompletedTask;
            });
            return this;
        }
    }
}
