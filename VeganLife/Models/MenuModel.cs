// <copyright file="MenuModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models
{
    using Newtonsoft.Json;

    public partial class MenuModel : ObservableObject
    {
        [JsonProperty("image_landscape")]
        public string ImgSource { get; set; }

        [JsonIgnore]
        public string Title { get; set; }
    }
}
