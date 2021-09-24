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
                var width = Console.WindowWidth;
                var stringBuilder = new StringBuilder(1000);
                _ = stringBuilder.Append('!', width - 1);
                _ = stringBuilder.AppendLine();
                _ = stringBuilder.AppendFormat(
                    "{0:yyyy/MM/dd tt hh:mm:ss.ffffff dddd}", DateTime.Now).AppendLine();
                _ = stringBuilder.Append("<--").Append(nameof(UnhandledTaskException)).AppendLine("-->");
                _ = stringBuilder.AppendLine(e.ToString());
                _ = stringBuilder.Append('*', width - 1);
                _ = stringBuilder.AppendLine();
                stringBuilder.ToString().WriteLine(ConsoleColor.Red);
            }

            UnhandledTaskException?.Invoke(null, e);
        }
    }
}
