// <copyright file="Channel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.GoogleNewsModels
{
    public class Channel
    {
        public string generator { get; set; }

        public string title { get; set; }

        public string link { get; set; }

        public string language { get; set; }

        public string webMaster { get; set; }

        public string copyright { get; set; }

        public string lastBuildDate { get; set; }

        public string description { get; set; }

        public List<Item> item { get; set; }
    }
}
