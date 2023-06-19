// <copyright file="Source.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.GoogleNewsModels
{
    using Newtonsoft.Json;

    public class Source
    {
        [JsonProperty("@url")]
        public string url { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }
}
