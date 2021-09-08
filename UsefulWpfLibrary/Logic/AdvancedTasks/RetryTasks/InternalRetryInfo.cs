using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    internal readonly struct InternalRetryInfo
    {
        public InternalRetryInfo(object info, uint totalRetriedCount, Exception exception)
        {
            dynamic d = info;
            var retryContext = new InternalRetryContext(exception,
                totalRetriedCount,
                (uint)d.RetriedCount);
            Type type = typeof(RetryInfo<>);
            _context = retryContext.Convert();
            if (info.GetType().GetGenericTypeDefinition() != type)
                throw new ArgumentException($"类型必须为{type}", nameof(info));
            _info = info;
        }
        public Type ExceptionType => _info.ExceptionType;
        public void AddRetriedCount()
        {
            _info.AddRetriedCount();
        }

        public Task<ContinueRetry> ContinueRetry(CancellationToken token)
        {
            var task =
                _info.ContinueRetry?.DynamicInvoke(token, _context)! as
                    Task<ContinueRetry>;
            return task!;
        }

        public async Task OnExceptionThrow(CancellationToken token)
        {
            if (_info.OnExceptionThrow?.DynamicInvoke(token, _context) is Task task)
                await task.ConfigureAwait(false);
        }

        public async Task OnRetry(CancellationToken token)
        {
            if (_info.OnRetry?.DynamicInvoke(token, _context) is Task task) await task.ConfigureAwait(false);
        }

        public async Task WaitTimeSpan(CancellationToken token)
        {
            if (_info.WaitTimeSpan?.DynamicInvoke(token, _context) is Task<TimeSpan>
                task)
            {
                TimeSpan timeSpan = await task.ConfigureAwait(false);
                await Task.Delay(timeSpan, token).ConfigureAwait(false);
            }
        }

        private readonly object _context;
        private readonly dynamic _info;
    }
}