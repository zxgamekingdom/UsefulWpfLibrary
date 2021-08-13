using System.Windows;

namespace UsefulWpfLibrary.Logic.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string? s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static bool IsNotNullOrWhiteSpace(this string? s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        public static void ShowErrorMessage(this string s)
        {
            MessageBox.Show(s, "´íÎó", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowInfoMessage(this string s)
        {
            MessageBox.Show(s, "ÐÅÏ¢", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarningMessage(this string s)
        {
            MessageBox.Show(s, "¾¯¸æ", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
