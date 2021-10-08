using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.Logic;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public record RetryResultTask<TResult>(Func<CancellationToken, Task<TResult>> Func,
        CancellationToken? Token)
    {
        private readonly Dictionary<Type, Func<Exception, uint, uint, object>>
            _createGenericRetryContexts = new();

        private readonly Dictionary<Type, uint> _exceptionCounters = new();

        private readonly
            List<(Type exceptionType, Delegate @delegate, TaskCreationOptions
                creationOptions, TaskScheduler scheduler)> _onExceptionThrowDelegates =
                new();

        private readonly List<(Type exceptionType, Delegate continueRetry, Delegate?
            delay, Delegate? onContinueRetry, bool isGeneric, TaskCreationOptions
            creationOptions, TaskScheduler scheduler)> _retryDelegates = new();

        private uint _totalRetriesCount;
        public bool IsRan { get; private set; }

        private void CheckNotRan()
        {
            if (IsRan) throw new InvalidOperationException("任务已经开始运行了,无法执行此操作");
        }

        private static void CheckIsExceptionType(Type exceptionType)
        {
            Type type = typeof(Exception);
            if (type.IsAssignableFrom(exceptionType) is false)
                throw new ArgumentException($"必须传入{type}及其子类类型");
        }

        public RetryResultTask<TResult> OnExceptionThrow(Type exceptionType,
            Func<Exception, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            CheckIsExceptionType(exceptionType);
            TryAddExceptionCounter(exceptionType);
            _onExceptionThrowDelegates.Add((exceptionType, func,
                creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        public RetryResultTask<TResult> OnExceptionThrow<TException>(
            Func<TException, CancellationToken, Task> func,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            return OnExceptionThrow(typeof(TException),
                (exception, token) => func.Invoke((TException)exception, token),
                creationOptions,
                scheduler);
        }

        public RetryResultTask<TResult> Retry(Type exceptionType,
            Func<RetryContext, CancellationToken, Task<bool>> continueRetry,
            Func<RetryContext, CancellationToken, Task<TimeSpan>>? delay = default,
            Func<RetryContext, CancellationToken, Task>? onContinueRetry = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            CheckNotRan();
            CheckIsExceptionType(exceptionType);
            TryAddExceptionCounter(exceptionType);
            _retryDelegates.Add((exceptionType, continueRetry, delay, onContinueRetry,
                false, creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        public RetryResultTask<TResult> Retry<TException>(
            Func<RetryContext<TException>, CancellationToken, Task<bool>> continueRetry,
            Func<RetryContext<TException>, CancellationToken, Task<TimeSpan>>? delay =
                default,
            Func<RetryContext, CancellationToken, Task>? onContinueRetry = default,
            TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null) where TException : Exception
        {
            CheckNotRan();
            var exceptionType = typeof(TException);
            CheckIsExceptionType(exceptionType);
            TryAddExceptionCounter(exceptionType);
            _retryDelegates.Add((exceptionType, continueRetry, delay, onContinueRetry,
                true, creationOptions.GetCreationOptions(), scheduler.GetScheduler()));
            return this;
        }

        private void TryAddExceptionCounter(Type exceptionType)
        {
            if (_exceptionCounters.ContainsKey(exceptionType) is false)
                _exceptionCounters.Add(exceptionType, 0);

            if (_createGenericRetryContexts.ContainsKey(exceptionType) is false)
                _createGenericRetryContexts.Add(exceptionType,
                    CreateGenericRetryContentFunc(exceptionType));
        }

        private async Task ExceptionThrow(Exception e,
            Type exceptionType,
            CancellationToken token)
        {
            foreach (var item in _onExceptionThrowDelegates.Where(item =>
                item.exceptionType.IsAssignableFrom(exceptionType)))
                await Task.Factory.StartNew(async () =>
                            await ((Task)item.@delegate.DynamicInvoke(e, token))
                                .ConfigureAwait(
                                    false),
                        token,
                        item.creationOptions,
                        item.scheduler)
                    .Unwrap()
                    .ConfigureAwait(false);
        }

        private async Task<bool> IsRetry(Type exceptionType,
            Exception e,
            CancellationToken token)
        {
            foreach (var item in _retryDelegates.Where(item =>
                item.exceptionType.IsAssignableFrom(exceptionType)))
            {
                var buff = await Task.Factory.StartNew(async () =>
                        {
                            var totalRetriesCount = _totalRetriesCount;
                            var thisExceptionRetriesCount =
                                _exceptionCounters[item.exceptionType];
                            object context = item.isGeneric switch
                            {
                                true => _createGenericRetryContexts[item.exceptionType]
                                    .Invoke(e,
                                        totalRetriesCount,
                                        thisExceptionRetriesCount),
                                false => new RetryContext(e,
                                    totalRetriesCount,
                                    thisExceptionRetriesCount)
                            };
                            var isContinueRetry =
                                await ((Task<bool>)item.continueRetry.DynamicInvoke(
                                    context,
                                    token)).ConfigureAwait(false);
                            if (isContinueRetry && (item.delay != null))
                            {
                                var timeSpan =
                                    await ((Task<TimeSpan>)item.delay.DynamicInvoke(
                                        context,
                                        token)).ConfigureAwait(false);
                                await Task.Delay(timeSpan, token).ConfigureAwait(false);
                            }

                            if (isContinueRetry && (item.onContinueRetry != null))
                            {
                                await (Task)item.onContinueRetry.DynamicInvoke(context,
                                    token);
                            }

                            return isContinueRetry;
                        },
                        token,
                        item.creationOptions,
                        item.scheduler)
                    .Unwrap()
                    .ConfigureAwait(false);
                if (buff) return true;
            }

            return false;
        }

        private static Func<Exception, uint, uint, object>
            CreateGenericRetryContentFunc(Type type)
        {
            var genericType = typeof(RetryContext<>).MakeGenericType(type);
            var constructorInfo = genericType.GetConstructors()[0];
            var dynamicMethod = new DynamicMethod("",
                typeof(object),
                new[] { typeof(Exception), typeof(uint), typeof(uint) });
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, type);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Newobj, constructorInfo);
            ilGenerator.Emit(OpCodes.Ret);
            return (Func<Exception, uint, uint, object>)dynamicMethod.CreateDelegate(
                typeof(Func<Exception, uint, uint, object>));
        }

        [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
        [SuppressMessage("CodeQuality", "IDE0079:请删除不必要的忽略", Justification = "<挂起>")]
        private void RunExceptionCounter(Type exceptionType)
        {
            _totalRetriesCount++;
            Type[] array = _exceptionCounters.Keys.ToArray();
            var length = array.Length;
            for (var i = 0; i < length; i++)
            {
                Type type = array[i];
                if (type.IsAssignableFrom(exceptionType)) _exceptionCounters[type]++;
            }
        }

        public Task<TResult> Run(TaskCreationOptions? creationOptions = null,
            TaskScheduler? scheduler = null)
        {
            var token = Token.GetToken();
            IsRan = true;
            return Task.Factory.StartNew(async () =>
                    {
                        while (true)
                            try
                            {
                                return await Func.Invoke(token).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                var exceptionType = e.GetType();
                                await ExceptionThrow(e, exceptionType, token)
                                    .ConfigureAwait(false);
                                var isRetry = await IsRetry(exceptionType, e, token)
                                    .ConfigureAwait(false);
                                if (isRetry)
                                {
                                    RunExceptionCounter(exceptionType);
                                    continue;
                                }

                                throw;
                            }
                    },
                    token,
                    creationOptions.GetCreationOptions(),
                    scheduler.GetScheduler())
                .Unwrap()!;
        }
    }
}
