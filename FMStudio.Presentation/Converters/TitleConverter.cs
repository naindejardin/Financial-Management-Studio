using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using FMStudio.Presentation.Properties;

namespace FMStudio.Presentation.Converters
{
    public class TitleConverter : IMultiValueConverter
    {
        private static readonly TitleConverter defaultInstance = new TitleConverter();

        public static TitleConverter Default { get { return defaultInstance; } }


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || !(values[0] is string) || !(values[1] == null || values[1] is string))
            {
                return DependencyProperty.UnsetValue;
            }

            string title = (string)values[0];
            string documentName = (string)values[1];

            if (!string.IsNullOrEmpty(documentName))
            {
                title = string.Format(culture, Resources.TitleFormatString, documentName, title);
            }

            return title;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
