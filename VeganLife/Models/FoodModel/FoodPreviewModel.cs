using Newtonsoft.Json;
using SQLite;

namespace VeganLife.Models.FoodModel
{
    public class FoodPreviewModel
    {
        [JsonIgnore]
        [PrimaryKey]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
