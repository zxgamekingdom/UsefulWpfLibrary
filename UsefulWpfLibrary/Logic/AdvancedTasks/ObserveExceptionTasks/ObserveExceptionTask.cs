﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static class ObserveExceptionTask
    {
        public static FuncInfo<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken token = default)
        {
            return new FuncInfo<TResult>(func, token);
        }

        public static FuncInfo<TResult> Create<TResult>(Func<Task<TResult>> func,
            CancellationToken token = default)
        {
            return new FuncInfo<TResult>(_ => func.Invoke(), token);
        }

        public static FuncInfo<TResult> Create<TResult>(Func<TResult> func,
            CancellationToken token = default)
        {
            return new FuncInfo<TResult>(_ => Task.FromResult(func.Invoke()), token);
        }

        public static FuncInfo<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken token = default)
        {
            return new FuncInfo<TResult>(cancellationToken =>
                    Task.FromResult(func.Invoke(cancellationToken)),
                token);
        }

        public static ActionInfo Create(Func<CancellationToken, Task> action,
            CancellationToken token = default)
        {
            return new ActionInfo(action, token);
        }

        public static ActionInfo Create(Action action,
            CancellationToken token = default)
        {
            return new ActionInfo(_ =>
                {
                    action.Invoke();
                    return Task.CompletedTask;
                },
                token);
        }

        public static ActionInfo Create(Action<CancellationToken> action,
            CancellationToken token = default)
        {
            return new ActionInfo(cancellationToken =>
                {
                    action.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                token);
        }

        public static ActionInfo Create(Func<Task> action,
            CancellationToken token = default)
        {
            return new ActionInfo(_ => action.Invoke(), token);
        }

        public static Task<TResult> Run<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(func, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task<TResult> Run<TResult>(Func<Task<TResult>> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(func, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task<TResult> Run<TResult>(Func<TResult> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(func, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task<TResult> Run<TResult>(Func<CancellationToken, TResult> func,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(func, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Func<CancellationToken, Task> action,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(action, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Action action,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(action, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Action<CancellationToken> action,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(action, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }

        public static Task Run(Func<Task> action,
            TaskCreationOptions? creationOptions = default,
            TaskScheduler? scheduler = default,
            CancellationToken token = default)
        {
            return Create(action, token)
                .SetCreationOptions(creationOptions)
                .SetScheduler(scheduler)
                .Run();
        }
    }
}
