using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.RetryTasks;
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
                var i = 0;
                await new RetryTaskBuilder().Create(_ =>
                    {
                        Console.Out.WriteLine("i = {0}", i);

                        i++;
                        switch (i % 2 == 0)
                        {
                            case true:
                                throw new DivideByZeroException();
                            case false:
                                throw new InvalidCastException();
                        }

                        return Task.FromResult(1);
                    })
                    .Retry<DivideByZeroException>((context, _) =>
                        {
                            context.WriteLine(ConsoleColor.Cyan);
                            return Task.FromResult(
                                context.ThisExceptionRetriesCount < 2);
                        },
                        (_, _) => Task.FromResult(TimeSpan.FromMilliseconds(1)))
                    .Retry<InvalidCastException>((context, _) =>
                        {
                            context.WriteLine(ConsoleColor.DarkYellow);
                            return Task.FromResult(
                                context.ThisExceptionRetriesCount < 2);
                        },
                        (_, _) => Task.FromResult(TimeSpan.FromMilliseconds(1)))
                    .Retry<Exception>((context, _) =>
                        {
                            context.WriteLine(ConsoleColor.Blue);
                            return Task.FromResult(context.ThisExceptionRetriesCount <
                                10);
                        },
                        (_, _) => Task.FromResult(TimeSpan.FromMilliseconds(1)))
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
