using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks
{
    public record HandleExceptionTask(Func<CancellationToken, Task> Func,
        CancellationToken? Token = null)
    {
        private readonly
            List<(Type exceptionType, Delegate @delegate, TaskCreationOptions
                creationOptions, TaskScheduler scheduler)> _handleDelegates = new();

        private readonly
            List<(Type exceptionType, Delegate @delegate, TaskCreationOptions
                creationOptions, TaskScheduler scheduler)> _onExceptionThrowDelegates =
                new();

        public bool IsRan { get; private set; }

        private void CheckNotRan()
        {
            if (IsRan) throw new InvalidOperationException("任务已经开始运行了,无法执行此操作");
        }

        public HandleExceptionTask OnExceptionThrow(Type exceptionType,
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

        public HandleExceptionTask OnExceptionThrow<TException>(
            Func<TException, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return OnExceptionThrow(typeof(TException),
                (exception, token) => func.Invoke((TException)exception, token),
                creationOptions,
                scheduler);
        }

        public HandleExceptionTask Handle<TException>(
            Func<TException, CancellationToken, Task<HandleResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return Handle(typeof(TException),
                (exception, token) => func.Invoke((TException)exception, token),
                creationOptions,
                scheduler);
        }

        public HandleExceptionTask Handle(Type exceptionType,
            Func<Exception, CancellationToken, Task<HandleResult>> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            CheckIsExceptionType(exceptionType);
            _handleDelegates.Add((exceptionType, func,
                creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        private static void CheckIsExceptionType(Type exceptionType)
        {
            Type type = typeof(Exception);
            if (type.IsAssignableFrom(exceptionType) is false)
                throw new ArgumentException($"必须传入{type}及其子类类型");
        }

        public Task Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            var token = Token.GetToken();
            IsRan = true;
            return Task.Factory.StartNew<Task>(async () =>
                    {
                        try
                        {
                            await Func.Invoke(token).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            await ExceptionThrow(e, token).ConfigureAwait(false);
                            var result = await HandleException(e, token)
                                .ConfigureAwait(false);
                            if (result is { IsHandle: true }) return;

                            throw;
                        }
                    },
                    token,
                    creationOptions.GetCreationOptions(),
                    scheduler.GetScheduler())
                .Unwrap()!;
        }

        private async Task<HandleResult?> HandleException(Exception e,
            CancellationToken token)
        {
            Type type = e.GetType();
            HandleResult? buff = null;
            foreach ((var exceptionType, var @delegate, var taskCreationOptions,
                var taskScheduler) in _handleDelegates)
            {
                if (exceptionType.IsAssignableFrom(type))
                {
                    var result = await RunSingleHandleLogic(e,
                            token,
                            @delegate,
                            taskCreationOptions,
                            taskScheduler)
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

        private static Task<HandleResult> RunSingleHandleLogic(Exception e,
            CancellationToken token,
            Delegate @delegate,
            TaskCreationOptions taskCreationOptions,
            TaskScheduler taskScheduler)
        {
            return Task.Factory.StartNew(async () =>
                        await ((Task<HandleResult>)@delegate.DynamicInvoke(e, token))
                            .ConfigureAwait(false),
                    token,
                    taskCreationOptions,
                    taskScheduler)
                .Unwrap();
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
