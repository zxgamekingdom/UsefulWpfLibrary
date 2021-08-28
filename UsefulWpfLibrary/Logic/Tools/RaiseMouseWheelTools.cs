using System.Windows;
using System.Windows.Input;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class RaiseMouseWheelTools
    {
        public static readonly DependencyProperty IsRaiseProperty =
            DependencyProperty.RegisterAttached("IsRaise",
                typeof(bool),
                typeof(RaiseMouseWheelTools),
                new PropertyMetadata(default(bool), PropertyChangedCallback));

        private static readonly DependencyProperty MouseWheelEventHandlerProperty =
            DependencyProperty.RegisterAttached("MouseWheelEventHandler",
                typeof(MouseWheelEventHandler),
                typeof(RaiseMouseWheelTools),
                new PropertyMetadata(default(MouseWheelEventHandler)));

        public static void ElementOnPreviewMouseWheel(object sender,
            MouseWheelEventArgs e)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var eventArgs = new MouseWheelEventArgs(e.MouseDevice,
                e.Timestamp,
                e.Delta);
            eventArgs.RoutedEvent = UIElement.MouseWheelEvent;
            eventArgs.Source = sender;
            ((FrameworkElement)sender).RaiseEvent(eventArgs);
        }

        public static bool GetIsRaise(FrameworkElement element)
        {
            return (bool)element.GetValue(IsRaiseProperty);
        }

        public static void SetIsRaise(FrameworkElement element, bool value)
        {
            element.SetValue(IsRaiseProperty, value);
        }

        private static void ElementOnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            element.Unloaded -= ElementOnUnloaded;
            RemoveMouseWheelEventHandler(element);
        }

        private static MouseWheelEventHandler GetMouseWheelEventHandler(
            DependencyObject element)
        {
            return (MouseWheelEventHandler)element.GetValue(
                MouseWheelEventHandlerProperty);
        }

        private static void PropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            var value = (bool)e.NewValue;
            if (value)
            {
                RemoveMouseWheelEventHandler(element);
                MouseWheelEventHandler handler = ElementOnPreviewMouseWheel;
                element.PreviewMouseWheel += handler;
                SetMouseWheelEventHandler(element, handler);
            }
            else
            {
                RemoveMouseWheelEventHandler(element);
            }

            element.Unloaded += ElementOnUnloaded;
        }

        private static void RemoveMouseWheelEventHandler(FrameworkElement element)
        {
            MouseWheelEventHandler eventHandler = GetMouseWheelEventHandler(element);
            element.PreviewMouseWheel -= eventHandler;
        }

        private static void SetMouseWheelEventHandler(DependencyObject element,
            MouseWheelEventHandler value)
        {
            element.SetValue(MouseWheelEventHandlerProperty, value);
        }
    }
}
