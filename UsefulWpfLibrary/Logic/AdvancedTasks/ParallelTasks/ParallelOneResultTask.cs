using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public record ParallelOneResultTask<TResult>(TResult Result,
        CancellationToken? Token = null) where TResult : class
    {
        private readonly
            ConcurrentBag<(Func<TResult, CancellationToken, Task> func,
                TaskCreationOptions, TaskScheduler)> _infos = new();

        public bool IsRan { get; private set; }

        public ParallelOneResultTask<TResult> Add(
            Func<TResult, CancellationToken, Task> func,
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

        public Task<TResult> Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            IsRan = true;
            var token = Token.GetToken();
            return Task.Factory.StartNew(async () =>
                    {
                        (Func<TResult, CancellationToken, Task> func,
                            TaskCreationOptions,
                            TaskScheduler)[] array = _infos.ToArray();
                        var length = array.Length;
                        var tasks = new Task[length];
                        for (var index = 0; index < length; index++)
                        {
                            (var func, var taskCreationOptions,
                                var taskScheduler) = array[index];
                            tasks[index] = Task.Factory.StartNew(async () =>
                                        await func.Invoke(Result, token)
                                            .ConfigureAwait(false),
                                    token,
                                    taskCreationOptions,
                                    taskScheduler)
                                .Unwrap();
                        }

                        try
                        {
                            await Task.WhenAll(tasks).ConfigureAwait(false);
                            return Result;
                        }
                        catch
                        {
                            // ignored
                        }

                        List<Exception> exceptions = new(length);
                        foreach (var task in tasks)
                        {
                            try
                            {
                                await task.ConfigureAwait(false);
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
