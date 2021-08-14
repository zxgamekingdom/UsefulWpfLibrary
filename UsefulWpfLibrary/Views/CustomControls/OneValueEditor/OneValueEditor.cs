using System.Windows;
using System.Windows.Controls;

namespace UsefulWpfLibrary.Views.CustomControls.OneValueEditor
{
    public class OneValueEditor : Control
    {
        public static readonly DependencyProperty ValueNameProperty =
            DependencyProperty.Register(nameof(ValueName),
                typeof(string),
                typeof(OneValueEditor),
                new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value),
                typeof(object),
                typeof(OneValueEditor),
                new FrameworkPropertyMetadata(default(object),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        static OneValueEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OneValueEditor),
                new FrameworkPropertyMetadata(typeof(OneValueEditor)));
        }
        public object Value
        {
            get => (object)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public string ValueName
        {
            get => (string)GetValue(ValueNameProperty);
            set => SetValue(ValueNameProperty, value);
        }
    }
}