using Newtonsoft.Json;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public class USDAFoodPreviewModel
    {
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
