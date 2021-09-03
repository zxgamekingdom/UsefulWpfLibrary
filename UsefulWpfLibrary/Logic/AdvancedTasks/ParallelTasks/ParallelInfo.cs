using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public record ParallelInfo(CancellationToken? Token = default)
    {
        private readonly ConcurrentBag<ActionInfo> _bag = new();
        public CheckTaskState TaskState { get; } = new();

        public Task Run()
        {
            TaskState.Start();
            return TaskState.Run(async () =>
            {
                Token?.ThrowIfCancellationRequested();
                ActionInfo[] actionInfos = _bag.ToArray();
                int length = actionInfos.Length;
                Task[] tasks = new Task[length];
                for (var i = 0; i < length; i++)
                {
                    tasks[i] = actionInfos[i].Run();
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
                    Task task = tasks[index];
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
            });
        }

        public ParallelInfo AddTask(Func<CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            TaskState.CheckNotStarted();
            _bag.Add(ObserveExceptionTask.Create(func, Token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler));
            return this;
        }

        public ParallelInfo AddTask(Action<CancellationToken> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default)
        {
            AddTask(token =>
            {
                func.Invoke(token);
                return Task.CompletedTask;
            });
            return this;
        }
    }
}
