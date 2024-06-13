using Newtonsoft.Json;
using SQLite;

namespace VeganLife.Models.BaseModel
{
    public class NutritionBaseModel
    {
        [JsonIgnore]
        [PrimaryKey]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }
}