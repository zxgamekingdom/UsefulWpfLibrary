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

        public static void DisposeDataContext(FrameworkElement? element)
        {
            if (element == null) return;
            object dataContext = element.DataContext;
            if (dataContext is not IDisposable disposable) return;
            switch (dataContext)
            {
                case ISetDataContextToolsOptions options
                    when options.Options.IsAutoDispose is false:
                    return;
                default:
                    disposable.Dispose();
                    element.DataContext = default;
                    break;
            }
        }

        public static Type GetDataContextType(FrameworkElement element)
        {
            return (Type)element.GetValue(DataContextTypeProperty);
        }

        public static void SetDataContext(FrameworkElement? element, Type? type)
        {
            if (element == null || type == null) return;
            object context = Ioc.Get(type);
            element.DataContext = context;
            if (context is ISetDataContextToolsOptions options)
                options.Options.SetOwendFrameworkElement(element);
        }

        public static void SetDataContextType(FrameworkElement element, Type value)
        {
            element.SetValue(DataContextTypeProperty, value);
        }

        private static void FrameworkElementOnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            DisposeDataContext(element);
            element.Unloaded -= FrameworkElementOnUnloaded;
        }

        private static void PropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement)d;
            SetDataContext((FrameworkElement)d, (Type)e.NewValue);
            element.Unloaded += FrameworkElementOnUnloaded;
        }
    }
}
