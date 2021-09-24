using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.HandleExceptionTasks;
using UsefulWpfLibrary.Logic.Extensions;
using UsefulWpfLibrary.Logic.Tools;

namespace WpfApp.Test
{
    public static class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            _ = ConsoleTools.AllocConsole();
            try
            {
                await HandleExceptionTaskTools
                    .Create(_ => throw new TaskCanceledException())
                    .OnExceptionThrow(typeof(Exception),
                        (exception, _) =>
                            exception.ToString().WriteLine(ConsoleColor.Red))
                    .Handle<Exception>((exception, _) =>
                    {
                        throw new TimeoutException("", exception);
                        return HandleResult.NotHandle();
                    })
                    .Run()
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _ = Console.ReadLine();
        }
    }
}
