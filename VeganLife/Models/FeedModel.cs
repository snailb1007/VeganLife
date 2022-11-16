namespace VeganLife.Models
{
    public class FeedModel
    {
        public string Title { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public string Author { get; set; }
        public string Thumbnail { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public object Enclosure { get; set; }
        public IList<string> Categories { get; set; }
    }
}
