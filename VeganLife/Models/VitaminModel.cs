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
        [JsonProperty("affiliates")]
        public string AffiliationsJson { get; set; }
    }

    public partial class VitaminModel
    {
        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Id);

        [Ignore]
        public List<AffiliationModel> Affiliations
        {
            get
            {
                return string.IsNullOrWhiteSpace(AffiliationsJson)
                    ? new List<AffiliationModel>()
                    : JsonConvert.DeserializeObject<List<AffiliationModel>>(AffiliationsJson) ?? Enumerable.Empty<AffiliationModel>().ToList();
            }

            set
            {
                AffiliationsJson = JsonConvert.SerializeObject(value);
            }
        }
    }

    //TODO
    public class AffiliationModel
    {
        [AutoIncrement]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("mall")]
        public bool IsMall { get; set; }
    }
}