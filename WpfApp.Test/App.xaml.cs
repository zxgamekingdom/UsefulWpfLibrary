using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UsefulWpfLibrary.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks;
using UsefulWpfLibrary.Logic.Extensions;
using UsefulWpfLibrary.Logic.Tools;

namespace WpfApp.Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Mutex _mutex;

        public App()
        {
            ConsoleTools.AllocConsole();
            Ioc.Init(registry =>
            {
            });
            InitializeComponent();
            _mutex = SingletonProgramTools.GenToken(
                "26CC549B-ACED-41DD-ADAF-6F252EC835F4");
            NewMethod1();
        }

        private static async Task NewMethod()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            await Task.Delay(100, cancellationToken);
        }

        private static async Task NewMethod1()
        {
            try
            {
                ParallelTask.IParallelTask parallelTask = ParallelTask.Create()
                    .AddTask(() => throw new InvalidOperationException())
                    .AddTask(NewMethod)
                    .AddTask(() => throw new DivideByZeroException())
                    .AddTask(() => throw new TaskCanceledException());
                await parallelTask.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
