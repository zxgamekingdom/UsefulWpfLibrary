using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks
{
    public struct RetryTaskBuilder
    {
        public  RetryResultTask<TResult> Create<TResult>(
            Func<CancellationToken, Task<TResult>> func,
            CancellationToken? token = default)
        {
            return new RetryResultTask<TResult>(func, token);
        }

        public  RetryTask Create(Func<CancellationToken, Task> func,
            CancellationToken? token = default)
        {
            return new RetryTask(func, token);
        }
    }
}
