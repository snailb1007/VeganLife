// Ignore Spelling: Fdc

using Newtonsoft.Json;
using SQLite;
using VeganLife.Models.BaseModel;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public class USDAFoodNutritionFactModel
    {
        [PrimaryKey]
        [JsonProperty("fdcId")]
        public int Id { get; set; }

        [JsonProperty("publicationDate")]
        public string PublicationDate { get; set; }

        [Ignore]
        public List<FoodNutrient> foodNutrients { get; set; }

        public string FoodNutrientsJsonData { get; set; }

        //public string? dataType { get; set; }

        //public string? foodClass { get; set; }

        //public List<object>? inputFoods { get; set; }

        //public FoodCategory? foodCategory { get; set; }
    }

    //public class FoodCategory
    //{
    //    [PrimaryKey]
    //    public int id { get; set; }

    //    public string code { get; set; }

    //    public string description { get; set; }
    //}

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

    //public class FoodNutrientDerivation
    //{
    //    [PrimaryKey]
    //    public int id { get; set; }

    //    public string? code { get; set; }

    //    public string? description { get; set; }

    //    public FoodNutrientSource? foodNutrientSource { get; set; }
    //}

    //public class FoodNutrientSource
    //{
    //    [PrimaryKey]
    //    public int id { get; set; }

    //    public string? code { get; set; }

    //    public string? description { get; set; }
    //}

    //public class FoodPortion
    //{
    //    [PrimaryKey]
    //    public int id { get; set; }

    //    public double gramWeight { get; set; }

    //    public int sequenceNumber { get; set; }

    //    public int amount { get; set; }

    //    public string? modifier { get; set; }

    //    public MeasureUnit? measureUnit { get; set; }

    //    public int? dataPoints { get; set; }
    //}

    //public class MeasureUnit
    //{
    //    [PrimaryKey]
    //    public int id { get; set; }

    //    public string? name { get; set; }

    //    public string? abbreviation { get; set; }
    //}

    public class Nutrient : BaseNutrientModel
    {
        [PrimaryKey]
        public int id { get; set; }

        //[JsonProperty("name")]
        //public string Name { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("unitName")]
        public string UnitName { get; set; }
    }

    //public partial class Nutrient
    //{
    //    public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Name ?? string.Empty);
    //}

    //public class NutrientConversionFactor
    //{
    //    [PrimaryKey]
    //    public int id { get; set; }

    //    public double proteinValue { get; set; }

    //    public double fatValue { get; set; }

    //    public double carbohydrateValue { get; set; }

    //    public string? type { get; set; }

    //    public string? name { get; set; }

    //    public double? value { get; set; }
    //}
}