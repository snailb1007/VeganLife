namespace VeganLife.Helpers.Converter
{
    internal class TitleForHyperlinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var rootString = value as string;
                var startIndex = rootString.IndexOf("[");
                value = rootString.Substring(startIndex + 1, rootString.IndexOf("]") - startIndex -1);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
