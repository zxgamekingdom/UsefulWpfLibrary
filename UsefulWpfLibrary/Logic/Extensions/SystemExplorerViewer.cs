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
            return info.Exists switch
            {
                false => throw new InvalidOperationException($@"路径""{info.FullName
                }""不存在,无法打开"),
                _ => Process.Start("explorer", info.FullName)
            };
        }
    }
}
