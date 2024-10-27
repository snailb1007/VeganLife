// <copyright file="MenuModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Newtonsoft.Json;

namespace VeganLife.Models
{
    /// <summary>
    /// Menu category in top.
    /// </summary>
    public partial class MenuModel
    {
        [JsonProperty("image_landscape")]
        public string ImgSource { get; set; }

        [JsonIgnore]
        public string Title { get; set; }
    }

    /// <summary>
    /// Partial to property changed.
    /// </summary>
    public partial class MenuModel : ObservableObject
    {
    }
}
