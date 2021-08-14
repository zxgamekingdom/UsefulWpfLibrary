using System.Runtime.InteropServices;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class ConsoleTools
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();
    }
}