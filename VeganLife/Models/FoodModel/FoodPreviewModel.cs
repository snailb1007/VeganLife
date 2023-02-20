using Newtonsoft.Json;

namespace VeganLife.Models.FoodModel
{
    public class FoodPreviewModel
    {
        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
