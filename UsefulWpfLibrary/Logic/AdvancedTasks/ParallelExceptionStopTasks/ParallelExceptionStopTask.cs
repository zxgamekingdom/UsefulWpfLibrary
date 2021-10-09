using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.Extensions;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelExceptionStopTasks
{
    public record ParallelExceptionStopTask(CancellationToken? Token = default)
    {
        private readonly ConcurrentBag<(Func<CancellationToken, Task> func,
            TaskCreationOptions creationOptions, TaskScheduler scheduler)> _infos =
            new();

        private Func<Exception, Task>? _onExceptionFunc;
        public bool IsRan { get; private set; }

        public ParallelExceptionStopTask OnException(Func<Exception, Task>? onException)
        {
            CheckNotRan();
            _onExceptionFunc = onException;
            return this;
        }

        public ParallelExceptionStopTask Add(Func<CancellationToken, Task> func,
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

        public Task Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            IsRan = true;
            var exceptionCts = new CancellationTokenSource();
            var (_, token) =
                Token.GetToken().Link(tokenSources: new[] { exceptionCts, });
            return Task.Factory.StartNew(async () =>
                    {
                        var array = _infos.ToArray();
                        var length = array.Length;
                        var tasks = new Task[length];
                        for (var index = 0; index < length; index++)
                        {
                            var (func, taskCreationOptions, taskScheduler) =
                                array[index];
                            tasks[index] = Task.Factory.StartNew(async () =>
                                        await func.Invoke(token).ConfigureAwait(false),
                                    token,
                                    taskCreationOptions,
                                    taskScheduler)
                                .Unwrap();
                        }

                        while (tasks.All(task => task.IsCompleted) is false)
                        {
                            try
                            {
                                foreach (var task in tasks)
                                {
                                    await task.ConfigureAwait(false);
                                }

                                await Task.Delay(1, token).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                if (_onExceptionFunc != null)
                                {
                                    await _onExceptionFunc.Invoke(e)
                                        .ConfigureAwait(false);
                                }
                            }
                            finally
                            {
                                exceptionCts.Cancel();
                                exceptionCts.Dispose();
                            }
                        }

                        try
                        {
                            await Task.WhenAll(tasks).ConfigureAwait(false);
                            return;
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
                    scheduler.GetScheduler())
                .Unwrap();
        }
    }
}
