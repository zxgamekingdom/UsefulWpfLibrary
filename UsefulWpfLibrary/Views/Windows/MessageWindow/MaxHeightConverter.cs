using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UsefulWpfLibrary.Views.Windows.MessageWindow
{
    internal sealed class MaxHeightConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) =>
            double.TryParse(value.ToString(), out var result) ? result / 3 : 500;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("General",
            "RCS1079:Throwing of new NotImplementedException.",
            Justification = "<¹ÒÆð>")]
        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
