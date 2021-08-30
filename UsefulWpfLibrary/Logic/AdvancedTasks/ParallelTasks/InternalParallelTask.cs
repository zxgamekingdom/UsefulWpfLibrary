﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks
{
    public static partial class ParallelTask
    {
        private class InternalParallelTask : IParallelTask
        {
            private readonly ConcurrentBag<ObserveExceptionTask.IActionTask> _bag =
                new();

            private CancellationToken? _cancellationToken;
            public CheckTaskState TaskState { get; } = new();

            private CancellationToken Token =>
                _cancellationToken ?? CancellationToken.None;

            public IParallelTask AddTask(Func<CancellationToken, Task> func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null)
            {
                TaskState.CheckState();
                _bag.Add(ObserveExceptionTask.Create(func)
                    .SetCancellationToken(cancellationToken)
                    .SetCreationOptions(creationOptions)
                    .SetScheduler(scheduler));
                return this;
            }

            public IParallelTask AddTask(Action<CancellationToken> func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null)
            {
                TaskState.CheckState();
                _bag.Add(ObserveExceptionTask.Create(func)
                    .SetCancellationToken(cancellationToken)
                    .SetCreationOptions(creationOptions)
                    .SetScheduler(scheduler));
                return this;
            }

            public IParallelTask AddTask(Action func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null)
            {
                TaskState.CheckState();
                _bag.Add(ObserveExceptionTask.Create(func)
                    .SetCancellationToken(cancellationToken)
                    .SetCreationOptions(creationOptions)
                    .SetScheduler(scheduler));
                return this;
            }

            public IParallelTask AddTask(Func<Task> func,
                CancellationToken? cancellationToken = null,
                TaskCreationOptions? creationOptions = null,
                TaskScheduler? scheduler = null)
            {
                TaskState.CheckState();
                _bag.Add(ObserveExceptionTask.Create(func)
                    .SetCancellationToken(cancellationToken)
                    .SetCreationOptions(creationOptions)
                    .SetScheduler(scheduler));
                return this;
            }

            public async Task Run()
            {
                Token.ThrowIfCancellationRequested();
                TaskState.Start();
                var tasks = new List<Task>(_bag.Count);
                foreach (ObserveExceptionTask.IActionTask actionTask in _bag)
                {
                    Token.ThrowIfCancellationRequested();
                    actionTask.SetCancellationToken(Token);
                    Task task = actionTask.Run();
                    tasks.Add(task);
                }

                Token.ThrowIfCancellationRequested();
                try
                {
                    await Task.WhenAll(tasks);
                }
                catch
                {
                    // ignored
                }

#pragma warning disable AsyncFixer02 // Long-running or blocking operations inside an async method
                Task.WaitAll(tasks.ToArray(), Token);
#pragma warning restore AsyncFixer02 // Long-running or blocking operations inside an async method
            }

            public IParallelTask SetCancellationToken(
                CancellationToken? cancellationToken)
            {
                _cancellationToken = cancellationToken;
                return this;
            }
        }
    }
}