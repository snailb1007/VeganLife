namespace VeganLife.Models
{
    public class ChatMessageModel
    {
        public required string Text { get; set; }
        public bool IsUserMessage { get; set; }
        public string? Avatar { get; set; }
    }
}
