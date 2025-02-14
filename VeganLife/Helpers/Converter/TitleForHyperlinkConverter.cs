namespace VeganLife.Helpers.Converter
{
    internal class TitleForHyperlinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return null;
                case string rootString:
                {
                    var startIndex = rootString.IndexOf('[');
                    value = rootString.Substring(startIndex + 1, rootString.IndexOf(']') - startIndex - 1);
                    break;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
