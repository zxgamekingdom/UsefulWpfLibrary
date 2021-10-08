using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public record ParallelResultTask<TResult>(CancellationToken? Token = null)
    {
        private readonly ConcurrentBag<(Func<CancellationToken, Task<TResult>> func,
            TaskCreationOptions creationOptions, TaskScheduler scheduler)> _infos =
            new();

        public bool IsRan { get; private set; }

        public ParallelResultTask<TResult> Add(
            Func<CancellationToken, Task<TResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            _infos.Add((func, creationOptions.GetCreationOptions(),
                scheduler.GetScheduler()));
            return this;
        }

        private void CheckNotRan()
        {
            if (IsRan) throw new InvalidOperationException("任务已经开始运行了,无法添加任务");
        }

        public TResult[]? Results { get; private set; }

        public Task<TResult[]> Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            IsRan = true;
            var token = Token.GetToken();
            return Task.Factory.StartNew(async () =>
                    {
                        var array = _infos.ToArray();
                        var length = array.Length;
                        var tasks = new Task<TResult>[length];
                        var results = new ConcurrentBag<TResult>();
                        for (var index = 0; index < length; index++)
                        {
                            (var func, var taskCreationOptions,
                                var taskScheduler) = array[index];
                            tasks[index] = Task.Factory.StartNew(async () =>
                                    {
                                        var result = await func.Invoke(token)
                                            .ConfigureAwait(false);
                                        results.Add(result);
                                        return result;
                                    },
                                    token,
                                    taskCreationOptions,
                                    taskScheduler)
                                .Unwrap();
                        }

                        try
                        {
                            return await Task.WhenAll(tasks).ConfigureAwait(false);
                        }
                        catch
                        {
                            // ignored
                        }
                        finally
                        {
                            Results = results.ToArray();
                        }

                        List<Exception> exceptions = new(length);
                        foreach (Task<TResult> task in tasks)
                        {
                            try
                            {
                                _ = await task.ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                exceptions.Add(e);
                            }
                        }

                        throw new AggregateException(exceptions);
                    },
                    token,
                    creationOptions.GetCreationOptions(),
                    scheduler.GetScheduler())
                .Unwrap();
        }
    }
}
