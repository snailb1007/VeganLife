// <copyright file="HotItemModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models
{
    using VeganLife.Models.GoogleNewsModels;

    public class HotItemModel : Item
    {
        public HotItemModel()
        {
        }

        public HotItemModel(Item item, string topic)
        {
            this.title = item.title;
            this.link = item.link;
            this.guid = item.guid;
            this.pubDate = item.pubDate;
            this.description = item.description;
            //this.source = item.source;
            this.ImageTitleUri = item.ImageTitleUri;
            this.Topic = topic;
        }

        public string Topic { get; set; }
    }
}
