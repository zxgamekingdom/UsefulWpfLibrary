using System;
using UsefulWpfLibrary.Logic.Extensions;

namespace UsefulWpfLibrary.Logic.TasksHelpers
{
    public static class TaskExceptionObserver
    {
        public static event EventHandler<Exception>? UnhandledTaskException;

        public static void OnUnhandledTaskException(Exception e)
        {
            $@"{nameof(UnhandledTaskException)}:{Environment.NewLine}{e}".WriteLine(
                ConsoleColor.Red);
            UnhandledTaskException?.Invoke(null, e);
        }
    }
}
