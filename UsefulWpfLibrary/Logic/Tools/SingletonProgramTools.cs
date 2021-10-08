using JetBrains.Annotations;
using System;
using System.Threading;
using System.Windows;
using UsefulWpfLibrary.Logic.Extensions;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class SingletonProgramTools
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:��ɾ������Ҫ�ĺ���", Justification = "<����>")]
        public static Mutex GenToken([NotNull] string identification)
        {
            if (string.IsNullOrWhiteSpace(identification))
            {
                throw new ArgumentException("Value cannot be null or whitespace.",
                    nameof(identification));
            }

            Mutex mutex = new(true, identification, out var @new);
            if (@new is false)
            {
                $@"����һ��Ψһʶ����Ϊ""{identification}""�������������,������޷�����".ShowErrorMessageBox();
                Application.Current.Shutdown(0);
                Environment.Exit(0);
                throw new InvalidOperationException();
            }

            return mutex;
        }
    }
}
