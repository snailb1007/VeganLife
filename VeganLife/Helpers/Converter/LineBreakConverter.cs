// <copyright file="LineBreakConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers.Converter
{
    public class LineBreakConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                value = (value as string).Replace("\\r\\n", Environment.NewLine + "· ");
                value = (value as string).Replace("\r\n", Environment.NewLine + "· ");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
