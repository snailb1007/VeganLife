namespace VeganLife.Helpers.Converter
{
    public class MarkdownMenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string text)
                return null;

            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var headers = new List<string>();
            int headerCount = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("#### "))
                {
                    headerCount++;
                    headers.Add($"{headerCount}. {line.Substring(5)}");
                }
            }

            return headers;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
