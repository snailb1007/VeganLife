using Newtonsoft.Json;

namespace VeganLife.Models
{
    public class VitaminModel
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("web_view")]
        public string WebView { get; set; }
    }
}
