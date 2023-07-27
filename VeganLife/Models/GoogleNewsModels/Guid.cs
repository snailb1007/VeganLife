// <copyright file="Guid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.GoogleNewsModels
{
    using Newtonsoft.Json;

    public class Guid
    {
        [JsonProperty("@isPermaLink")]
        public string isPermaLink { get; set; }

        [JsonProperty("#text")]
        public string text { get; set; }
    }
}
