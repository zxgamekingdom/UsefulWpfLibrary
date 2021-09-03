using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public record CollectionResultParallelInfo<TResult>(CancellationToken? Token =
        default)
    {
        private readonly ConcurrentBag<FuncInfo<TResult>> _bag = new();
        private ReadOnlyCollection<TResult>? _results;

        public CheckTaskState TaskState { get; } = new();

        public ReadOnlyCollection<TResult>? Results
        {
            get
            {
                TaskState.CheckFinished();
                return _results;
            }
            private set => _results = value;
        }

        public Task<ReadOnlyCollection<TResult>> Run()
        {
            TaskState.Start();
            return TaskState.Run(async () =>
            {
                Token?.ThrowIfCancellationRequested();
                FuncInfo<TResult>[] funcInfos = _bag.ToArray();
                int length = funcInfos.Length;
                var tasks = new Task<TResult>[length];
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
                List<TResult> results = new(length);
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var index = 0; index < tasks.Length; index++)
                {
                    var task = tasks[index];
                    try
                    {
                        TResult result = await task.ConfigureAwait(false);
                        results.Add(result);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }

                var buffResults = results.AsReadOnly();
                Results = buffResults;
                if (exceptions.Count != 0)
                {
                    throw new AggregateException(exceptions);
                }

                return buffResults;
            });
        }

        public CollectionResultParallelInfo<TResult> AddTask(
            Func<CancellationToken, Task<TResult>> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            TaskState.CheckNotStarted();
            _bag.Add(ObserveExceptionTask.Create(func, Token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler));
            return this;
        }

        public CollectionResultParallelInfo<TResult> AddTask(
            Func<CancellationToken, TResult> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            AddTask(token => Task.FromResult(func.Invoke(token)));
            return this;
        }
    }
}
