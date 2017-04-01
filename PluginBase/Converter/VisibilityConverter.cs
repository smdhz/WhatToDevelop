using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WhatToDevelop.Converter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                if (System.Convert.ToInt32(value) == -1)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
            else if (value is bool)
                return System.Convert.ToBoolean(value) ? Visibility.Visible : Visibility.Collapsed;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
    }
}
