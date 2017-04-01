using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WhatToDevelop.Converter
{
    [ValueConversion(typeof(int[]), typeof(string))]
    public class ArrayConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is int[]))
                return "Incorrect";
            return string.Join(",", (value as int[]).Select(i => i.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string))
                return null;
            try
            { return (value as string).Split(',').Select(i => System.Convert.ToInt32(i)).ToArray(); }
            catch (FormatException)
            { return null; }
        }
    }
}
