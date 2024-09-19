// <copyright file="UndefinedMacroFoodNutriFactModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using SQLite;
using VeganLife.Helpers;

namespace VeganLife.Models
{
    public class UndefinedMacroFoodNutriFactModel
    {
        [PrimaryKey]
        [JsonIgnore]
        [JsonProperty("FdcId")]
        public string Id { get; set; }

        public string Name { get; set; }

        [Ignore]
        public List<UndefinedFoodNutrient> foodNutrients { get; set; }

        public string FoodNutrientsJsonData { get; set; }
    }

    public class UndefinedFoodNutrient
    {
        public double? Amount { get; set; }

        [JsonProperty("nutrient")]
        public UndefinedNutrient Nutrient { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
    }

    public partial class UndefinedNutrient
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class UndefinedNutrient
    {
        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Name ?? string.Empty);
    }
}
