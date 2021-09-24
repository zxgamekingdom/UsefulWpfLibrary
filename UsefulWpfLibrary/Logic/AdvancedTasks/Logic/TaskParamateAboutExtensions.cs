using System.Threading;
using System.Threading.Tasks;

namespace UsefulWpfLibrary.Logic.AdvancedTasks.Logic
{
    internal static class TaskParamateAboutExtensions
    {
        public static CancellationToken GetToken(this CancellationToken? token)
        {
            return token ?? CancellationToken.None;
        }

        public static TaskCreationOptions GetCreationOptions(
            this TaskCreationOptions? creationOptions)
        {
            return creationOptions ?? TaskCreationOptions.None;
        }

        public static TaskScheduler GetScheduler(this TaskScheduler? scheduler)
        {
            return scheduler ?? TaskScheduler.Current;
        }
    }
}
