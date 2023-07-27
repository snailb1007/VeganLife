// <copyright file="Rss.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.GoogleNewsModels
{
    using Newtonsoft.Json;

    public class Rss
    {
        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@xmlns:media")]
        public string xmlnsmedia { get; set; }

        public Channel channel { get; set; }
    }
}
