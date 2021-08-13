using System;
using System.Windows;
using UsefulWpfLibrary.Logic.ViewModels;

namespace UsefulWpfLibrary.Logic.Tools
{
    public static class DataContextTools
    {
        public static readonly DependencyProperty DataContextTypeProperty =
            DependencyProperty.RegisterAttached("DataContextType",
                typeof(Type),
                typeof(DataContextTools),
                new PropertyMetadata(default(Type), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement) d;
            DisposeDataContext(element);
            var type = (Type) e.NewValue;
            element.DataContext = Ioc.Get(type);
            if (element.DataContext is ISetDataContextToolsOptions options)
                options.Options.SetOwendFrameworkElement(element);
            element.Unloaded += FrameworkElementOnUnloaded;
        }

        private static void FrameworkElementOnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            DisposeDataContext(element);
            element.Unloaded -= FrameworkElementOnUnloaded;
        }

        private static void DisposeDataContext(FrameworkElement element)
        {
            object dataContext = element.DataContext;
            if (dataContext is IDisposable disposable)
            {
                if (dataContext is ISetDataContextToolsOptions options &&
                    options.Options.IsAutoDispose is false)
                {
                    return;
                }

                disposable.Dispose();
                element.DataContext = default;
            }
        }

        public static void SetDataContextType(FrameworkElement element, Type value)
        {
            element.SetValue(DataContextTypeProperty, value);
        }

        public static Type GetDataContextType(FrameworkElement element)
        {
            return (Type) element.GetValue(DataContextTypeProperty);
        }
    }
}
