using System;
using System.Threading.Tasks;
using UsefulWpfLibrary.Logic.Tools;

namespace WpfApp.Test
{
    public static class Program
    {
        [STAThread]
        public static async Task Main(string[] args)
        {
            ConsoleTools.AllocConsole();

            Console.ReadLine();
        }
    }
}
