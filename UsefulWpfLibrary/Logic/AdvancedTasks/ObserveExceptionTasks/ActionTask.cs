using System;
using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        private class ActionTask : AbstractActionTask
        {
            public ActionTask(Action action) : base(Converter(action))
            {
            }

            private static Func<CancellationToken, Task> Converter(Action action)
            {
                return _ =>
                {
                    action.Invoke();
                    return Task.CompletedTask;
                };
            }
        }
    }
}
