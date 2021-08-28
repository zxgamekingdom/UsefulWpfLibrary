using System;
using System.Diagnostics;
using System.IO;

namespace UsefulWpfLibrary.Logic.Extensions
{
    public static class SystemExplorerViewer
    {
        public static Process? ViewerPath(FileSystemInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            if (info.Exists is false)
            {
                throw new InvalidOperationException($@"路径""{info.FullName}""不存在,无法打开");
            }

            return Process.Start("explorer", info.FullName);
        }
    }
}
