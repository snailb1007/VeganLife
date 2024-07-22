// <copyright file="VitaminModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Newtonsoft.Json;
using SQLite;
using VeganLife.Helpers;
using VeganLife.Models.BaseModel;

namespace VeganLife.Models
{
    public partial class VitaminModel : NutritionBaseModel
    {
    }

    public partial class VitaminModel
    {
        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Id);
    }

    //TODO
    public class AffiliationModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string NutrientName { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public bool Mall { get; set; }

        public string Price { get; set; }
    }
}