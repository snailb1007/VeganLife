// <copyright file="VitaminModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models
{
    using Newtonsoft.Json;

    public class VitaminModel
    {
        [JsonIgnore]
        required public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("web_view")]
        public string WebView { get; set; }
    }
}
