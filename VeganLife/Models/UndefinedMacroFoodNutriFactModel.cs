// <copyright file="UndefinedMacroFoodNutriFactModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using VeganLife.Helpers;

namespace VeganLife.Models
{
    public class UndefinedMacroFoodNutriFactModel
    {
        [JsonIgnore]
        public string FdcId { get; set; }

        public string Name { get; set; }

        public List<UndefinedFoodNutrient> foodNutrients { get; set; }
    }

    public class UndefinedFoodNutrient
    {
        public double? Amount { get; set; }

        [DataMember(Name = "nutrient")]
        public UndefinedNutrient Nutrient { get; set; }

        [DataMember(Name = "unit")]
        public string Unit { get; set; }
    }

    public partial class UndefinedNutrient
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    public partial class UndefinedNutrient
    {
        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Name ?? string.Empty);
    }
}
