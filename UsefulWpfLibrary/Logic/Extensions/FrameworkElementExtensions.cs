using System.Windows;

namespace UsefulWpfLibrary.Logic.Extensions
{
    public static class FrameworkElementExtensions
    {
        public static Window? GetWindow(this FrameworkElement element)
        {
            return element is Window window ? window : Window.GetWindow(element);
        }
    }
}