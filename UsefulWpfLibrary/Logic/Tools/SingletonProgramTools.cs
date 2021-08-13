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
            Justification = "<����>")]
        public static Mutex GenToken([NotNull] string identification)
        {
            if (string.IsNullOrWhiteSpace(identification))
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(identification));
            Mutex mutex = new(true, identification, out bool @new);
            if (@new is false)
            {
                $@"����һ��Ψһʶ����Ϊ""{identification}""�������������,������޷�����".ShowErrorMessage();
                Application.Current.Shutdown(0);
                Environment.Exit(0);
                throw new InvalidOperationException();
            }

            return mutex;
        }
    }
}
