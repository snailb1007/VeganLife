// <copyright file="FoodDetailModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using SQLite;

namespace VeganLife.Models.FoodModel
{
    public class FoodDetailModel
    {
        [JsonIgnore]
        [PrimaryKey]
        public string Id { get; set; }

        [JsonProperty("decore")]
        public string Decorate { get; set; }

        public string Ingredient { get; set; }

        public string Making { get; set; }

        public string Sauce { get; set; }

        [JsonProperty("note")]
        public string Note { get;set; }

        [JsonProperty("root_link")]
        public string RootLink { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
