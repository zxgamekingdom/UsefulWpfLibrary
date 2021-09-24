using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public record ParallelTask(CancellationToken? Token = null)
    {
        private readonly ConcurrentBag<(Func<CancellationToken, Task> func,
            TaskCreationOptions creationOptions, TaskScheduler scheduler)> _infos =
            new();

        public bool IsRan { get; private set; }

        public ParallelTask Add(Func<CancellationToken, Task> func,
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

        public ParallelTask Add(Action<CancellationToken> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            _infos.Add((token =>
            {
                func.Invoke(token);
                return Task.CompletedTask;
            }, creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        public Task Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            IsRan = true;
            var token = Token.GetToken();
            return Task.Factory.StartNew(async () =>
                {
                    var array = _infos.ToArray();
                    var length = array.Length;
                    var tasks = new Task[length];
                    for (var index = 0; index < length; index++)
                    {
                        (var func, var taskCreationOptions, var taskScheduler) =
                            array[index];
                        tasks[index] = Task.Factory.StartNew(async () =>
                                    await func.Invoke(token).ConfigureAwait(false),
                                token,
                                taskCreationOptions,
                                taskScheduler)
                            .Unwrap();
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
                    foreach (Task task in tasks)
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
                scheduler.GetScheduler());
        }
    }
}
