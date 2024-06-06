using VeganLife.Models.ConverterModels;

namespace VeganLife.Helpers.Converter
{
    public class MarkdownMenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string text)
            {
                return null;
            }

            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var headers = new List<MarkdownMenuModel>();
            byte headerCount = 0;

            foreach (var line in lines)
            {
                if (line.StartsWith("#### "))
                {
                    headerCount++;
                    headers.Add(new MarkdownMenuModel()
                    {
                        Content = $"{headerCount}. {line.Substring(5)}",
                        Id = headerCount,
                    });
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
