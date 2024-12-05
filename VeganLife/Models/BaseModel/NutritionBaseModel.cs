using Newtonsoft.Json;
using VeganLife.Helpers;

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

        [JsonProperty("category")]
        public string Category { get; set; }

        public DateTime DateTime => DateTime.ParseExact(this.Date, "yyyyMMdd", CultureInfo.InvariantCulture);

        public string TimeAgo => DateTimeHelper.CalculateTimeAgo(this.DateTime);
    }
}