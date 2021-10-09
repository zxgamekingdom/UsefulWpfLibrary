using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.ParallelExceptionStopTasks;
using UsefulWpfLibrary.Logic.AdvancedTasks.ParallelTasks;
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
           
            _ = Console.ReadLine();
        }
    }
}
