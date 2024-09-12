// Ignore Spelling: Fdc

using SQLite;
using System.Runtime.Serialization;
using VeganLife.Helpers;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public class USDAFoodNutritionFactModel
    {
        [PrimaryKey]
        [DataMember(Name = "fdcId")]
        public int Id { get; set; } = -1;

        [DataMember(Name = "publicationDate")]
        public string? PublicationDate { get; set; }

        public List<FoodNutrient>? foodNutrients { get; set; }

        //public string? dataType { get; set; }

        //public string? foodClass { get; set; }

        //public List<object>? inputFoods { get; set; }

        //public FoodCategory? foodCategory { get; set; }
    }

    public class FoodCategory
    {
        [PrimaryKey]
        public int id { get; set; }

        public string code { get; set; }

        public string description { get; set; }
    }

    public class FoodNutrient
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int? id { get; set; } = -1;

        [DataMember(Name = "nutrient")]
        public Nutrient? Nutrient { get; set; }

        public string? type { get; set; }

        [DataMember(Name = "amount")]
        public double? Amount { get; set; }

        public int? dataPoints { get; set; }

        public double? max { get; set; }

        public double? min { get; set; }
    }

    public class FoodNutrientDerivation
    {
        [PrimaryKey]
        public int id { get; set; }

        public string? code { get; set; }

        public string? description { get; set; }

        public FoodNutrientSource? foodNutrientSource { get; set; }
    }

    public class FoodNutrientSource
    {
        [PrimaryKey]
        public int id { get; set; }

        public string? code { get; set; }

        public string? description { get; set; }
    }

    public class FoodPortion
    {
        [PrimaryKey]
        public int id { get; set; }

        public double gramWeight { get; set; }

        public int sequenceNumber { get; set; }

        public int amount { get; set; }

        public string? modifier { get; set; }

        public MeasureUnit? measureUnit { get; set; }

        public int? dataPoints { get; set; }
    }

    public class MeasureUnit
    {
        [PrimaryKey]
        public int id { get; set; }

        public string? name { get; set; }

        public string? abbreviation { get; set; }
    }

    public partial class Nutrient
    {
        [PrimaryKey]
        public int id { get; set; }

        [DataMember(Name = "name")]
        public string? Name { get; set; }

        public int rank { get; set; }

        public string? unitName { get; set; }
    }

    public partial class Nutrient
    {
        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Name ?? string.Empty);
    }

    public class NutrientConversionFactor
    {
        [PrimaryKey]
        public int id { get; set; }

        public double proteinValue { get; set; }

        public double fatValue { get; set; }

        public double carbohydrateValue { get; set; }

        public string? type { get; set; }

        public string? name { get; set; }

        public double? value { get; set; }
    }
}
