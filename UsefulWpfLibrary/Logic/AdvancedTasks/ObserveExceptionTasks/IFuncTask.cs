using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks
{
    public static partial class ObserveExceptionTask
    {
        public interface IFuncTask<TResult>
        {
            Task<TResult> Run();

            IFuncTask<TResult> SetCancellationToken(
                CancellationToken? cancellationToken);

            IFuncTask<TResult> SetCreationOptions(TaskCreationOptions? creationOptions);
            IFuncTask<TResult> SetScheduler(TaskScheduler? scheduler);
        }
    }
}
