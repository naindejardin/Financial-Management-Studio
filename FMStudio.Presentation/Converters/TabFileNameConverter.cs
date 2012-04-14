﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.IO;

namespace FMStudio.Presentation.Converters
{
    public class TabFileNameConverter : IMultiValueConverter
    {
        private const int MaxCharacters = 40;

        private static TabFileNameConverter defaultInstance = new TabFileNameConverter();

        public static TabFileNameConverter Default { get { return defaultInstance; } }


        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || !(values[0] is string) || !(values[1] is bool))
            {
                return DependencyProperty.UnsetValue;
            }

            string fileName = Path.GetFileName((string)values[0]);
            if (fileName.Length > MaxCharacters)
            {
                fileName = fileName.Remove(MaxCharacters - 3) + "...";
            }

            bool modified = (bool)values[1];
            return fileName + (modified ? "*" : "");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
