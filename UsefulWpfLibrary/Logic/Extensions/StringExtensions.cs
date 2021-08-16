using System.Windows;
using UsefulWpfLibrary.Views.Windows.MessageWindow;

namespace UsefulWpfLibrary.Logic.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotNullOrWhiteSpace(this string? s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        public static bool IsNullOrWhiteSpace(this string? s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static void ShowErrorMessage(this string s)
        {
            MessageWindowTools.Show(s, messageType: MessageType.错误);
        }

        public static void ShowErrorMessageBox(this string s)
        {
            MessageBox.Show(s, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowInfoMessage(this string s)
        {
            MessageWindowTools.Show(s, messageType: MessageType.信息);
        }

        public static void ShowInfoMessageBox(this string s)
        {
            MessageBox.Show(s, "信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarningMessage(this string s)
        {
            MessageWindowTools.Show(s, messageType: MessageType.警告);
        }

        public static void ShowWarningMessageBox(this string s)
        {
            MessageBox.Show(s, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
