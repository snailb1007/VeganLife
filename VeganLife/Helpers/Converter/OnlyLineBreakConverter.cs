namespace VeganLife.Helpers.Converter;

public class OnlyLineBreakConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return string.Empty;
        }

        value = (value as string)?.Replace("\\r\\n", Environment.NewLine);
        value = ((string)value)?.Replace("\r\n", Environment.NewLine);
        value = ((string)value)?.Replace("\n", Environment.NewLine);

        return value ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
