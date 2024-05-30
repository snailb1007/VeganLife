// <copyright file="VitaminModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace VeganLife.Models
{
    public class VitaminModel
    {
        [JsonIgnore]
        required public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
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
