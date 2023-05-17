namespace VeganLife.Helpers.Converter
{
    class LineBreakConverter : IValueConverter
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
