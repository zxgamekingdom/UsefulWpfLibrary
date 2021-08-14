using System.Windows;

namespace UsefulWpfLibrary.Logic.ViewModels
{
    public class DataContextToolsOptions
    {
        public bool IsAutoDispose { get; set; } = true;
        public FrameworkElement? OwendFrameworkElement { get; private set; }
        public void SetOwendFrameworkElement(FrameworkElement element)
        {
            OwendFrameworkElement = element;
        }
    }
}