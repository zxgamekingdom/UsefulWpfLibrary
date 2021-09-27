using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public static class RetryTaskTools
    {
        public static RetryResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new RetryResultTask<TResult>(func, token);
        }

        public static RetryResultTask<TResult> Create<TResult>(
            Func<CancellationToken, TResult> func,
            CancellationToken? token = default)
        {
            return Create<TResult>(cancellationToken =>
                    Task.FromResult<TResult>(func.Invoke(cancellationToken)),
                token);
        }


        public static RetryTask Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new RetryTask(func, token);
        }

        public static RetryTask Create(Action<CancellationToken> action,
            CancellationToken? token = default)
        {
            return Create(cancellationToken =>
                {
                    action.Invoke(cancellationToken);
                    return Task.CompletedTask;
                },
                token);
        }
    }
}
