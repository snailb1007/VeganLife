using ExCSS;
using VeganLife.Models.ConverterModels;

namespace VeganLife.Helpers.Converter
{
    public class MarkdownStylingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string text)
            {
                return null;
            }

            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var processedLines = new List<MarkdownStylingModel>();
            byte headerCount = 0;

            foreach (var line in lines)
            {
                var sectinon = new MarkdownStylingModel();

                if (line.StartsWith("#### "))
                {
                    headerCount++;
                    sectinon.Id = headerCount;
                    sectinon.FormattedString.Spans.Add(new Span
                    {
                        Text = $"{headerCount}. ",
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 20,
                    });
                    sectinon.FormattedString.Spans.Add(new Span
                    {
                        Text = line.Substring(5),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 20,
                    });
                }
                else if (line.StartsWith("### "))
                {
                    sectinon.FormattedString.Spans.Add(new Span
                    {
                        Text = line.Substring(4),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 24,
                    });
                }
                else
                {
                    // Handle bold text within lines
                    var parts = System.Text.RegularExpressions.Regex.Split(line, @"(\*\*.*?\*\*)");
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("**") && part.EndsWith("**"))
                        {
                            sectinon.FormattedString.Spans.Add(new Span
                            {
                                Text = part[2..^2],
                                FontAttributes = FontAttributes.Bold,
                                FontSize = 16,
                            });
                        }
                        else
                        {
                            sectinon.FormattedString.Spans.Add(new Span { Text = part, FontSize = 16 });
                        }
                    }
                }

                processedLines.Add(sectinon);
            }

            return processedLines;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}