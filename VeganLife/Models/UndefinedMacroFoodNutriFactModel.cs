// <copyright file="UndefinedMacroFoodNutriFactModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Newtonsoft.Json;

using VeganLife.Helpers;
using VeganLife.Models.BaseModel;

namespace VeganLife.Models
{
    public class UndefinedMacroFoodNutriFactModel : BaseFoodModel
    {
        [PrimaryKey]
        [JsonIgnore]
        [JsonProperty("FdcId")]
        public string Id { get; set; }

        public string Name { get; set; }

        [Ignored]
        public List<UndefinedFoodNutrient> foodNutrients { get; set; }

        [JsonIgnore]
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

    public partial class UndefinedNutrient : BaseNutrientModel
    {
    }
}
