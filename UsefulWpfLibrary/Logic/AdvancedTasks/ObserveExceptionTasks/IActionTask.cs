using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        public interface IActionTask
        {
            Task Run();
            IActionTask SetCancellationToken(CancellationToken? cancellationToken);
            IActionTask SetCreationOptions(TaskCreationOptions? creationOptions);
            IActionTask SetScheduler(TaskScheduler? scheduler);
        }
    }
}
