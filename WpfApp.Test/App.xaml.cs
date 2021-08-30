using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UsefulWpfLibrary.Logic;
using UsefulWpfLibrary.Logic.AdvancedTasks.ObserveExceptionTasks;
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
            ObserveExceptionTask.Run(ct =>
                {
                    return test(ct);
                },
                new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);
        }

        private int test(CancellationToken ct)
        {
            int i = 0;
            var stackTrace = new StackTrace();
            foreach (StackFrame stackFrame in stackTrace.GetFrames())
            {
                MethodBase methodBase = stackFrame.GetMethod();
                new { methodBase.Name, methodBase.DeclaringType.FullName }.WriteLine(
                    ConsoleColor.Cyan);
            }

            return 1 / i;
        }
    }
}
