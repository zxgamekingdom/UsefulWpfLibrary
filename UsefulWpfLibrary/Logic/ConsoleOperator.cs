using System.Runtime.InteropServices;

namespace UsefulWpfLibrary.Logic
{
    public static class ConsoleOperator
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}
