using System.Diagnostics;
using System.IO;

namespace UsefulWpfLibrary.Logic.Extensions
{
    public static class FileSystemInfoExtensions
    {
        public static bool IsExists(this FileSystemInfo info)
        {
            return info.Exists;
        }

        public static bool IsNotExists(this FileSystemInfo info)
        {
            return info.Exists is false;
        }

        public static void CreateIsNotExists(this DirectoryInfo info)
        {
            if (info.IsNotExists())
            {
                info.Create();
            }
        }

        public static string CreateIsNotExists(this string directoryPath)
        {
            var info = directoryPath.ToDirectoryInfo();
            if (info.IsNotExists())
            {
                info.Create();
            }

            return directoryPath;
        }

        public static DirectoryInfo ToDirectoryInfo(this string path)
        {
            return new DirectoryInfo(path);
        }

        public static FileInfo ToFileInfo(this string path)
        {
            return new FileInfo(path);
        }

        public static Process? ViewerUseSystemExplorer(this FileSystemInfo info)
        {
            return SystemExplorerViewer.ViewerPath(info);
        }
    }
}
