using System;
using System.Threading.Tasks;
using System.Xaml;
using System.Xml;

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
            ConsoleTools.AllocConsole();
            try
            {
                var i = 0;
                await RetryTask.Create(token =>
                    {
                        i++;
                        if (i % 2 == 0)
                        {
                            throw new XamlDuplicateMemberException();
                        }
                        else
                        {
                            throw new XmlException();
                        }
                    })
                    .HandleException<Exception>()
                    .Retry(10,
                        onRetry: (token, context) =>
                        {
                            ConsoleExtensions.ConsoleSplitLine();
                            (context with { Exception = default }).WriteLine(
                                ConsoleColor
                                    .DarkGreen);
                            ConsoleExtensions.ConsoleSplitLine();
                            return Task.CompletedTask;
                        })
                    // .HandleException<XamlDuplicateMemberException>()
                    // .Retry(10,
                    //     onRetry: (token, context) =>
                    //     {
                    //         ConsoleExtensions.ConsoleSplitLine();
                    //         (context with { Exception = default }).WriteLine(
                    //             ConsoleColor.Cyan);
                    //         ConsoleExtensions.ConsoleSplitLine();
                    //         return Task.CompletedTask;
                    //     })
                    // .HandleException<XmlException>()
                    // .Retry(10,
                    //     onRetry: (token, context) =>
                    //     {
                    //         ConsoleExtensions.ConsoleSplitLine();
                    //         (context with { Exception = default }).WriteLine(
                    //             ConsoleColor.Red);
                    //         ConsoleExtensions.ConsoleSplitLine();
                    //         return Task.CompletedTask;
                    //     })
                    .Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}
