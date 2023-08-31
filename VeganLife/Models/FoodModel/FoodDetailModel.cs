// <copyright file="FoodDetailModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.FoodModel
{
    using Newtonsoft.Json;
    using SQLite;

    public class FoodDetailModel
    {
        [JsonIgnore ,PrimaryKey]
        public string Id { get; set; }

        public string Decorate { get; set; }

        public string Ingredient { get; set; }

        public string Making { get; set; }

        public string Sauce { get; set; }

        public string Note { get;set; }
    }
}
