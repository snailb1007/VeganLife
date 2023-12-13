using Newtonsoft.Json;
using SQLite;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public class USDAFoodNutritionFactModel
    {
        [PrimaryKey]
        [JsonProperty("fdcId")]
        public int FdcId { get; set; }
        // public string footnote { get; set; }
        // public string description { get; set; }
        public string publicationDate { get; set; }
        public List<FoodNutrient> foodNutrients { get; set; }
        //public List<FoodPortion> foodPortions { get; set; }
        public string dataType { get; set; }
        public string foodClass { get; set; }
        //public string scientificName { get; set; }
        //public List<object> foodComponents { get; set; }
        //public List<object> foodAttributes { get; set; }
        //public List<NutrientConversionFactor> nutrientConversionFactors { get; set; }
        public List<object> inputFoods { get; set; }
        //public int ndbNumber { get; set; }
        //public bool isHistoricalReference { get; set; }
        public FoodCategory foodCategory { get; set; }
    }

    public class FoodCategory
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class FoodNutrient
    {
        [JsonProperty("nutrient")]
        public Nutrient Nutrient { get; set; }
        public string type { get; set; }
        // public FoodNutrientDerivation foodNutrientDerivation { get; set; }
        public int? id { get; set; }
        [JsonProperty("amount")]
        public double? Amount { get; set; }
        [JsonIgnore]
        public int? dataPoints { get; set; }
        [JsonIgnore]
        public double? max { get; set; }
        [JsonIgnore]
        public double? min { get; set; }
    }

    public class FoodNutrientDerivation
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public FoodNutrientSource foodNutrientSource { get; set; }
    }

    public class FoodNutrientSource
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class FoodPortion
    {
        public int id { get; set; }
        public double gramWeight { get; set; }
        public int sequenceNumber { get; set; }
        public int amount { get; set; }
        public string modifier { get; set; }
        public MeasureUnit measureUnit { get; set; }
        public int? dataPoints { get; set; }
    }

    public class MeasureUnit
    {
        public int id { get; set; }
        public string name { get; set; }
        public string abbreviation { get; set; }
    }

    public class Nutrient
    {
        public int id { get; set; }
        //public string number { get; set; }
        public string name { get; set; }
        [JsonIgnore]
        public int rank { get; set; }
        public string unitName { get; set; }
    }

    public class NutrientConversionFactor
    {
        public int id { get; set; }
        public double proteinValue { get; set; }
        public double fatValue { get; set; }
        public double carbohydrateValue { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public double? value { get; set; }
    }
}
