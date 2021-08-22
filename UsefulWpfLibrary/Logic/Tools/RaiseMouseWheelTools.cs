using System.Windows;
using System.Windows.Input;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class RaiseMouseWheelTools
    {
        public static readonly DependencyProperty IsRaiseMouseWheelProperty =
            DependencyProperty.RegisterAttached("IsRaiseMouseWheel",
                typeof(bool),
                typeof(RaiseMouseWheelTools),
                new PropertyMetadata(default(bool), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            var value = (bool)e.NewValue;
            if (value is false) return;
            element.PreviewMouseWheel += (_, args) =>
            {
                // ReSharper disable once UseObjectOrCollectionInitializer
                var eventArgs = new MouseWheelEventArgs(args.MouseDevice,
                    args.Timestamp,
                    args.Delta);
                eventArgs.RoutedEvent = UIElement.MouseWheelEvent;
                eventArgs.Source = element;
                element.RaiseEvent(eventArgs);
            };
        }

        public static void SetIsRaiseMouseWheel(FrameworkElement element, bool value)
        {
            element.SetValue(IsRaiseMouseWheelProperty, value);
        }

        public static bool GetIsRaiseMouseWheel(FrameworkElement element)
        {
            return (bool)element.GetValue(IsRaiseMouseWheelProperty);
        }
    }
}
