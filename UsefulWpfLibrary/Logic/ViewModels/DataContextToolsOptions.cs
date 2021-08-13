using System.Windows;

namespace UsefulWpfLibrary.Logic.ViewModels
{
    public class DataContextToolsOptions
    {
        public void SetOwendFrameworkElement(FrameworkElement element)
        {
            OwendFrameworkElement = element;
        }

        public FrameworkElement? OwendFrameworkElement { get; private set; }

        public bool IsAutoDispose { get; init; } = true;
    }
}
