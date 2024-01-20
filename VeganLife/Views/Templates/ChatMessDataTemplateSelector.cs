namespace VeganLife.Views.Templates
{
    public class ChatMessDataTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate UserMessageItemTemplate { get; set; }
        public required DataTemplate BotMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = (ChatMessageModel)item;

            return message.IsUserMessage ? UserMessageItemTemplate : BotMessageTemplate;
        }
    }
}
