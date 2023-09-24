namespace VeganLife.Helpers.Converter
{
    internal class TitleForHyperlinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var rootString = value as string;
                value = rootString.Substring(rootString.IndexOf("["), rootString.IndexOf("]"));
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
