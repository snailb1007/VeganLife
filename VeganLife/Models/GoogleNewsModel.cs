using Newtonsoft.Json;

namespace VeganLife.Models
{
    public class GoogleNewsModel
    {
        public Rss rss { get; set; }
    }

    public class AtomLink
    {
        [JsonProperty("@href")]
        public string href { get; set; }

        [JsonProperty("@rel")]
        public string rel { get; set; }

        [JsonProperty("@type")]
        public string type { get; set; }
    }

    public class Channel
    {
        public Title title { get; set; }
        public Description description { get; set; }
        public string link { get; set; }
        public Image image { get; set; }
        public string generator { get; set; }
        public string lastBuildDate { get; set; }

        [JsonProperty("atom:link")]
        public AtomLink atomlink { get; set; }
        public Language language { get; set; }
        public List<Item> item { get; set; }
    }

    public class DcCreator
    {
        [JsonProperty("#cdata-section")]
        public string cdatasection { get; set; }
    }

    public class Description
    {
        [JsonProperty("#cdata-section")]
        public string cdatasection { get; set; }
    }

    public class Guid
    {
        [JsonProperty("@isPermaLink")]
        public string isPermaLink { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }

    public class Image
    {
        public string url { get; set; }
        public string title { get; set; }
        public string link { get; set; }
    }

    public class Item
    {
        public Title title { get; set; }
        public Description description { get; set; }
        public string link { get; set; }
        public Guid guid { get; set; }

        [JsonProperty("dc:creator")]
        public DcCreator dccreator { get; set; }
        public string pubDate { get; set; }

        [JsonProperty("media:content")]
        public MediaContent mediacontent { get; set; }
    }

    public class Language
    {
        [JsonProperty("#cdata-section")]
        public string cdatasection { get; set; }
    }

    public class MediaContent
    {
        [JsonProperty("@medium")]
        public string medium { get; set; }

        [JsonProperty("@url")]
        public string url { get; set; }
    }

    public class Root
    {
        public Rss rss { get; set; }
    }

    public class Rss
    {
        [JsonProperty("@xmlns:dc")]
        public string xmlnsdc { get; set; }

        [JsonProperty("@xmlns:content")]
        public string xmlnscontent { get; set; }

        [JsonProperty("@xmlns:atom")]
        public string xmlnsatom { get; set; }

        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@xmlns:media")]
        public string xmlnsmedia { get; set; }
        public Channel channel { get; set; }
    }

    public class Title
    {
        [JsonProperty("#cdata-section")]
        public string cdatasection { get; set; }
    }
}
