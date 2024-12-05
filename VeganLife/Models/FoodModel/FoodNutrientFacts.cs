// <copyright file="FoodNutrientFacts.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: Carb
using Newtonsoft.Json;


namespace VeganLife.Models.FoodModel
{
    public class FoodNutrientFacts
    {
        [JsonIgnore]
        [PrimaryKey]
        public string Id { get; set; }

        public ushort Calories { get; set; }

        public float Carb { get; set; }

        public float Fat { get; set; }

        public float Protein { get; set; }

        public ushort Sodium { get; set; }

        public ushort A { get; set; }

        public ushort Iron { get; set; }

        public ushort Magnesium { get; set; }

        public ushort Zinc { get; set; }

        public ushort B1 { get; set; }

        public ushort B3 { get; set; }

        public ushort B9 { get; set; }

        public ushort E { get; set; }

        public ushort Calcium { get; set; }

        public ushort D { get; set; }

        public ushort Kali { get; set; } // Potassium

        public ushort Phosphorus { get; set; }

        public ushort B2 { get; set; }

        public ushort B6 { get; set; }

        public ushort B12 { get; set; }

        public ushort K { get; set; }
    }
}
