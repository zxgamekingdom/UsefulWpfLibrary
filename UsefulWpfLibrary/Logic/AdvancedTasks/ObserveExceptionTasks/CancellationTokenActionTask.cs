using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class CancellationTokenActionTask : AbstractActionTask
        {
            public CancellationTokenActionTask(Action<CancellationToken> action) : base(
                Converter(action))
            {
            }

            private static Func<CancellationToken, Task> Converter(
                Action<CancellationToken> action)
            {
                return token =>
                {
                    action.Invoke(token);
                    return Task.CompletedTask;
                };
            }
        }
    }
}
