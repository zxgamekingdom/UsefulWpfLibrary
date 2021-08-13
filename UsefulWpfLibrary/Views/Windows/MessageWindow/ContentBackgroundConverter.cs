using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace UsefulWpfLibrary.Views.Windows.MessageWindow
{
    class ContentBackgroundConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return value is MessageType messageType ?
                messageType switch
                {
                    MessageType.��Ϣ => new SolidColorBrush(Colors.LightSkyBlue),
                    MessageType.���� => new SolidColorBrush(Colors.LightGoldenrodYellow),
                    MessageType.���� => new SolidColorBrush(Colors.PaleVioletRed),
                    _ => throw new ArgumentOutOfRangeException()
                } :
                value;
        }

        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
