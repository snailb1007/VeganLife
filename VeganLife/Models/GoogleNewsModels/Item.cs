// <copyright file="Item.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models.GoogleNewsModels
{
    using VeganLife.Helpers;

    public class Item
    {
        public string title { get; set; }

        public string link { get; set; }

        public Guid guid { get; set; }

        public string pubDate { get; set; }

        public string description { get; set; }

        // public Source source { get; set; }

        // custom
        public string ImageTitleUri { get; set; }

        // public DateTime LocalTimePosted => DateTimeHelper.GetDateTime(this.pubDate);
        public DateTime LocalTimePosted { get; set; }

        public string TimeAgoDisplay => DateTimeHelper.CalculateTimeAgo(this.LocalTimePosted);
    }
}
