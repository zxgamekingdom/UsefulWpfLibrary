using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using UsefulWpfLibrary.Logic.Tools;

namespace UsefulWpfLibrary.Logic.Behaviors
{
    public class DataContextBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type),
                typeof(Type),
                typeof(DataContextBehavior),
                new PropertyMetadata(default(Type), PropertyChangedCallback));

        public Type? Type
        {
            get => (Type)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        protected override void OnAttached()
        {
            DataContextTools.SetDataContext(AssociatedObject, Type);
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            DataContextTools.DisposeDataContext(AssociatedObject);
            base.OnDetaching();
        }

        private static void PropertyChangedCallback(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            DataContextTools.SetDataContext(
                ((DataContextBehavior)d).AssociatedObject,
                (Type)e.NewValue);
        }
    }
}
