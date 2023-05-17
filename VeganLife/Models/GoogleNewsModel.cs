using Newtonsoft.Json;
using VeganLife.Helpers;

namespace VeganLife.Models
{
    public class GoogleNewsModel
    {
        public Rss rss { get; set; }
    }

    public class Channel
    {
        public string generator { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string language { get; set; }
        public string webMaster { get; set; }
        public string copyright { get; set; }
        public string lastBuildDate { get; set; }
        public string description { get; set; }
        public List<Item> item { get; set; }
    }

    public class Guid
    {
        [JsonProperty("@isPermaLink")]
        public string isPermaLink { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Item
    {
        public string title { get; set; }
        public string link { get; set; }
        public Guid guid { get; set; }
        public string pubDate { get; set; }
        public string description { get; set; }
        public Source source { get; set; }

        // custom
        public string ImageTitleUri { get; set; }
        public DateTime LocalTimePosted => DateTimeHelper.GetDateTime(pubDate);
        public string TimeAgoDisplay => DateTimeHelper.CalcuteTimeAgo(LocalTimePosted);
    }

    public class Rss
    {
        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@xmlns:media")]
        public string xmlnsmedia { get; set; }
        public Channel channel { get; set; }
    }

    public class Source
    {
        [JsonProperty("@url")]
        public string url { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }
}
