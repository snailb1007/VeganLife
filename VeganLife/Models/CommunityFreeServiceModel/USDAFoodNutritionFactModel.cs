// Ignore Spelling: Fdc

using Newtonsoft.Json;

using VeganLife.Models.BaseModel;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public partial class USDAFoodNutritionFactModel : BaseFoodModel
    {
        [PrimaryKey]
        [JsonProperty("fdcId")]
        public int Id { get; set; }

        [JsonProperty("publicationDate")]
        public string PublicationDate { get; set; }

        [Ignored]
        public List<FoodNutrient> foodNutrients { get; set; }

        [JsonIgnore]
        public string FoodNutrientsJsonData { get; set; }
    }

    public class FoodNutrient
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nutrient")]
        public Nutrient Nutrient { get; set; }

        public string type { get; set; }

        public int? dataPoints { get; set; }

        public double? max { get; set; }

        public double? min { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }
    }

    public class Nutrient : BaseNutrientModel
    {
        [PrimaryKey]
        public int id { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("unitName")]
        public string UnitName { get; set; }
    }
}