namespace VeganLife.Models
{
    public class HotItemModel : Item
    {
        public HotItemModel()
        {
        }

        public HotItemModel(Item item, string topic)
        {
            title = item.title;
            link = item.link;
            guid = item.guid;
            pubDate = item.pubDate;
            description = item.description;
            source = item.source;
            ImageTitleUri = item.ImageTitleUri;
            Topic = topic;
        }

        public string Topic { get; set; }
    }
}
