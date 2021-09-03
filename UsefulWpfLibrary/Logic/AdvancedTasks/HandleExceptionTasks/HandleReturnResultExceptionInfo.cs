using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public record HandleReturnResultExceptionInfo<TResult>(
        Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token = default) :
        TaskInfo<HandleReturnResultExceptionInfo<TResult>>(Token)
    {
        private readonly
            ConcurrentBag<(Func<Exception, CancellationToken, Task> func,
                TaskCreationOptions? creationOptions, TaskScheduler? scheduler)>
            _onExceptionThrow = new();

        private readonly ConcurrentBag<(
                Func<Exception, CancellationToken, Task<HandleReturnResult<TResult>>>
                func,
                TaskCreationOptions? creationOptions, TaskScheduler? scheduler)>
            _onReturnDefualtResult = new();

        public bool IsFuncThrowException { get; private set; }

        private async Task TriggerOnExceptionThrow(Exception e)
        {
            foreach ((Func<Exception, CancellationToken, Task>? func,
                TaskCreationOptions? creationOptions,
                TaskScheduler? scheduler) in _onExceptionThrow)
                await ObserveExceptionTask.Run(token => func.Invoke(e, token),
                    creationOptions,
                    scheduler);
        }

        public Task<TResult?> Run()
        {
            TaskState.Start();
            return TaskState.Run(async () =>
            {
                Token?.ThrowIfCancellationRequested();
                try
                {
                    return await ObserveExceptionTask.Run(Func,
                        GetCreationOptions(),
                        GetScheduler(),
                        Token);
                }
                catch (Exception e)
                {
                    IsFuncThrowException = true;
                    await TriggerOnExceptionThrow(e);
                    foreach ((var func, TaskCreationOptions? creationOptions,
                        TaskScheduler? scheduler) in _onReturnDefualtResult)
                    {
                        var result = await ObserveExceptionTask.Run(token =>
                                func.Invoke(e, token),
                            creationOptions,
                            scheduler);
                        if (result.IsHandle is true) return result.Result;
                    }

                    throw;
                }
            });
        }

        public HandleReturnResultExceptionInfo<TResult> OnExceptionThrow<TException>(
            Func<TException, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return OnExceptionThrow((exception, token) => exception is TException e ?
                    func.Invoke(e, token) :
                    Task.CompletedTask,
                creationOptions,
                scheduler);
        }

        public HandleReturnResultExceptionInfo<TResult> OnExceptionThrow<TException>(
            Action<TException, CancellationToken> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return OnExceptionThrow<TException>((exception, token) =>
            {
                func.Invoke(exception, token);
                return Task.CompletedTask;
            });
        }

        public HandleReturnResultExceptionInfo<TResult> OnExceptionThrow(
            Action<Exception, CancellationToken> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return OnExceptionThrow((exception, token) =>
                {
                    func.Invoke(exception, token);
                    return Task.CompletedTask;
                },
                creationOptions,
                scheduler);
        }

        public HandleReturnResultExceptionInfo<TResult> OnExceptionThrow(
            Func<Exception, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            TaskState.CheckNotStarted();
            _onExceptionThrow.Add((func, creationOptions, scheduler));
            return this;
        }

        public HandleReturnResultExceptionInfo<TResult> HandleException(
            Func<Exception, CancellationToken, Task<HandleReturnResult<TResult>>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            TaskState.CheckNotStarted();
            _onReturnDefualtResult.Add((func, creationOptions, scheduler));
            return this;
        }

        public HandleReturnResultExceptionInfo<TResult> HandleException(
            Func<Exception, CancellationToken, HandleReturnResult<TResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return HandleException((exception, token) =>
                    Task.FromResult(func.Invoke(exception, token)),
                creationOptions,
                scheduler);
        }

        public HandleReturnResultExceptionInfo<TResult> HandleException<TException>(
            Func<TException, CancellationToken, Task<HandleReturnResult<TResult>>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return HandleException((exception, token) =>
                    exception is TException e ?
                        func.Invoke(e, token) :
                        Task.FromResult(HandleReturnResult<TResult>.NotHandle()),
                creationOptions,
                scheduler);
        }

        public HandleReturnResultExceptionInfo<TResult> HandleException<TException>(
            Func<TException, CancellationToken, HandleReturnResult<TResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return HandleException((exception, token) =>
                    exception is TException e ?
                        Task.FromResult(func.Invoke(e, token)) :
                        Task.FromResult(HandleReturnResult<TResult>.NotHandle()),
                creationOptions,
                scheduler);
        }
    }
}
