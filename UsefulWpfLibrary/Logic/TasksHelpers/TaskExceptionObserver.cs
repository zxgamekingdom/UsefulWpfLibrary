using System;
using System.Text;
using UsefulWpfLibrary.Logic.Extensions;

namespace UsefulWpfLibrary.Logic.TasksHelpers
{
    public static class TaskExceptionObserver
    {
        public static event EventHandler<Exception>? UnhandledTaskException;
        public static bool IsPrintOnConsole { get; set; } = true;

        public static void OnUnhandledTaskException(Exception? e)
        {
            if (e == null) return;
            if (IsPrintOnConsole)
            {
                int width = Console.WindowWidth;
                var stringBuilder = new StringBuilder(1000);
                stringBuilder.Append('!', width - 1);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine(
                    $"{DateTime.Now:yyyy/MM/dd tt hh:mm:ss.ffffff dddd}");
                stringBuilder.AppendLine($"<--{nameof(UnhandledTaskException)}-->");
                stringBuilder.AppendLine(e.ToString());
                stringBuilder.Append('*', width - 1);
                stringBuilder.AppendLine();
                stringBuilder.ToString().WriteLine(ConsoleColor.Red);
            }

            UnhandledTaskException?.Invoke(null, e);
        }
    }
}
