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
            MessageBox.Show(s, "����", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowInfoMessage(this string s)
        {
            MessageBox.Show(s, "��Ϣ", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarningMessage(this string s)
        {
            MessageBox.Show(s, "����", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
