namespace VeganLife.Models.ConverterModels
{
    public class MarkdownStylingModel
    {
        public byte Id { get; set; }

        public FormattedString FormattedString { get; set; } = new FormattedString();
    }
}
