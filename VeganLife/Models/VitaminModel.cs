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
        //[JsonIgnore]
        //[PrimaryKey]
        //public string Id { get; set; }

        //[JsonProperty("content")]
        //public string Content { get; set; }

        //[JsonProperty("date")]
        //public string Date { get; set; }
    }

    public partial class VitaminModel
    {
        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Id);

        public DateTime DateTime => DateTime.ParseExact(this.Date, "yyyyMMdd", CultureInfo.InvariantCulture);

        public string TimeAgo => DateTimeHelper.CalculateTimeAgo(this.DateTime);
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
