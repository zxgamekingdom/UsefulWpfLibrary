using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public static class RetryTask
    {
        public static CreateResultTaskInfo<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new CreateResultTaskInfo<TResult>(func, token);
        }

        public static CreateResultTaskInfo<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                    Task.FromResult(func.Invoke(cancellationToken)),
                token);
        }

        public static CreateTaskInfo Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new CreateTaskInfo(func, token);
        }

        public static CreateTaskInfo Create(Action<CancellationToken> func,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    func.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                token);
        }
    }
}
