// <copyright file="FoodDetailModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.FoodModel
{
    using SQLite;

    public class FoodDetailModel
    {
        [PrimaryKey]
        public ushort Id { get; set; }

        public string Decorate { get; set; }

        public string Ingredient { get; set; }

        public string Making { get; set; }

        public string Sauce { get; set; }
    }
}
