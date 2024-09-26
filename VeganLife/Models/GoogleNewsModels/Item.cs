// <copyright file="Item.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;

namespace VeganLife.Models.GoogleNewsModels
{
    public class Item
    {
#pragma warning disable SA1300 // Element should begin with upper-case letter
        public string title { get; set; }

        public string link { get; set; }

        public Guid guid { get; set; }

        public string pubDate { get; set; }

        public string description { get; set; }
#pragma warning restore SA1300 // Element should begin with upper-case letter

        // public Source source { get; set; }

        // custom
        public string ImageTitleUri { get; set; }

        // public DateTime LocalTimePosted => DateTimeHelper.GetDateTime(this.pubDate);
        public DateTime LocalTimePosted { get; set; }

        public string TimeAgoDisplay => DateTimeHelper.CalculateTimeAgo(this.LocalTimePosted);
    }
}
