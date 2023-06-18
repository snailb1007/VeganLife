// <copyright file="BookmarkFoodModelMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Messages
{
    using CommunityToolkit.Mvvm.Messaging.Messages;
    using VeganLife.Models.FoodModel;

    public class BookmarkFoodModelMessage : ValueChangedMessage<FoodPreviewModel>
    {
        public BookmarkFoodModelMessage(FoodPreviewModel value)
            : base(value)
        {
        }
    }
}
