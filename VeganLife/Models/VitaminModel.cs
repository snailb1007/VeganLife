// <copyright file="VitaminModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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

    // TODO
    //public class AffiliationModel
    //{
    //    [JsonIgnore]
    //    required public string Id { get; set; }

    //    [JsonProperty("name")]
    //    public string Name { get; set; }

    //    [JsonProperty("link")]
    //    public string Link { get; set; }
    //}
}
