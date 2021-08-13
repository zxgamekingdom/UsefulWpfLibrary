using System;
using System.Threading;
using System.Windows;
using JetBrains.Annotations;
using UsefulWpfLibrary.Logic.Extensions;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class SingletonProgramTools
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Wrong Usage",
            "DF0010:Marks undisposed local variables.",
            Justification = "<挂起>")]
        public static Mutex GenToken([NotNull] string identification)
        {
            if (string.IsNullOrWhiteSpace(identification))
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(identification));
            Mutex mutex = new(true, identification, out bool @new);
            if (@new is false)
            {
                $@"已有一个唯一识别码为""{identification}""的软件正在运行,本软件无法运行".ShowErrorMessage();
                Application.Current.Shutdown(0);
                Environment.Exit(0);
                throw new InvalidOperationException();
            }

            return mutex;
        }
    }
}
