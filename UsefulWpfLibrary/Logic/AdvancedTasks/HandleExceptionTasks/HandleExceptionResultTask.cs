using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public record HandleExceptionResultTask<TResult>(
        Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token = null)
    {
        private void CheckNotRan()
        {
            if (IsRan) throw new InvalidOperationException("任务已经开始运行了,无法执行此操作");
        }

        private readonly
            List<(Type exceptionType, Delegate @delegate, TaskCreationOptions
                creationOptions, TaskScheduler scheduler)> _onExceptionThrowDelegates =
                new();

        private readonly
            List<(Type exceptionType, Delegate @delegate, TaskCreationOptions
                creationOptions, TaskScheduler scheduler)> _handleDelegates = new();

        public bool IsRan { get; private set; }

        public HandleExceptionResultTask<TResult> OnExceptionThrow<TException>(
            Func<TException, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return OnExceptionThrow(typeof(TException),
                (exception, token) => func.Invoke((TException)exception, token),
                creationOptions,
                scheduler);
        }

        public HandleExceptionResultTask<TResult> OnExceptionThrow<TException>(
            Action<TException, CancellationToken> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return OnExceptionThrow<TException>((exception, token) =>
                {
                    func.Invoke(exception, token);
                    return Task.CompletedTask;
                },
                creationOptions,
                scheduler);
        }

        public HandleExceptionResultTask<TResult> OnExceptionThrow(Type exceptionType,
            Func<Exception, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            CheckIsExceptionType(exceptionType);
            _onExceptionThrowDelegates.Add((exceptionType, func,
                creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        public HandleExceptionResultTask<TResult> OnExceptionThrow(Type exceptionType,
            Action<Exception, CancellationToken> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return OnExceptionThrow(exceptionType,
                (exception, token) =>
                {
                    func.Invoke(exception, token);
                    return Task.CompletedTask;
                },
                creationOptions,
                scheduler);
        }

        public HandleExceptionResultTask<TResult> Handle<TException>(
            Func<TException, CancellationToken, Task<HandleResult<TResult>>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return Handle(typeof(TException),
                (exception, token) => func.Invoke((TException)exception, token),
                creationOptions,
                scheduler);
        }

        public HandleExceptionResultTask<TResult> Handle<TException>(
            Func<TException, CancellationToken, HandleResult<TResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return Handle(typeof(TException),
                (exception, token) =>
                    Task.FromResult(func.Invoke((TException)exception, token)),
                creationOptions,
                scheduler);
        }

        public HandleExceptionResultTask<TResult> Handle(Type exceptionType,
            Func<Exception, CancellationToken, Task<HandleResult<TResult>>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            CheckIsExceptionType(exceptionType);
            _handleDelegates.Add((exceptionType, func,
                creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        public HandleExceptionResultTask<TResult> Handle(Type exceptionType,
            Func<Exception, CancellationToken, HandleResult<TResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            return Handle(exceptionType,
                (exception, token) => Task.FromResult(func.Invoke(exception, token)),
                creationOptions,
                scheduler);
        }

        private static void CheckIsExceptionType(Type exceptionType)
        {
            Type type = typeof(Exception);
            if (type.IsAssignableFrom(exceptionType) is false)
                throw new ArgumentException($"必须传入{type}及其子类类型");
        }

        public Task<TResult> Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            var token = Token.GetToken();
            IsRan = true;
            return Task.Factory.StartNew<Task<TResult>>(async () =>
                    {
                        try
                        {
                            return await Func.Invoke(token).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            await ExceptionThrow(e, token).ConfigureAwait(false);
                            var result = await HandleException(e, token)
                                .ConfigureAwait(false);
                            if (result is { IsHandle: true })
                            {
                                return result.Result ??
                                    throw new InvalidOperationException();
                            }

                            throw;
                        }
                    },
                    token,
                    creationOptions.GetCreationOptions(),
                    scheduler.GetScheduler())
                .Unwrap()!;
        }

        private async Task<HandleResult<TResult?>?> HandleException(Exception e,
            CancellationToken token)
        {
            Type type = e.GetType();
            HandleResult<TResult?>? buff = null;
            foreach ((var exceptionType, var @delegate, var taskCreationOptions,
                var taskScheduler) in _handleDelegates)
            {
                if (exceptionType.IsAssignableFrom(type))
                {
                    var result = await Task.Factory.StartNew(async () =>
                                await ((Task<HandleResult<TResult?>>)@delegate
                                    .DynamicInvoke(e,
                                        token)).ConfigureAwait(false),
                            token,
                            taskCreationOptions,
                            taskScheduler)
                        .Unwrap()
                        .ConfigureAwait(false);
                    if (result.IsHandle)
                    {
                        buff = result;
                        break;
                    }
                }
            }

            return buff;
        }

        private async Task ExceptionThrow(Exception e, CancellationToken token)
        {
            foreach ((var exceptionType, var @delegate, var taskCreationOptions,
                var taskScheduler) in _onExceptionThrowDelegates)
            {
                var type = e.GetType();
                if (exceptionType.IsAssignableFrom(type))
                {
                    await Task.Factory.StartNew(async () =>
                                await ((Task)@delegate.DynamicInvoke(e, token))
                                    .ConfigureAwait(
                                        false),
                            token,
                            taskCreationOptions,
                            taskScheduler)
                        .Unwrap()
                        .ConfigureAwait(false);
                }
            }
        }
    }
}
