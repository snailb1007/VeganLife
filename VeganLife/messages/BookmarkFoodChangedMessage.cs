// <copyright file="BookmarkFoodChangedMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Messages
{
    using CommunityToolkit.Mvvm.Messaging.Messages;
    using VeganLife.Models.FoodModel;

    public class BookmarkFoodChangedMessage : ValueChangedMessage<FoodPreviewModel>
    {
        public BookmarkFoodChangedMessage(FoodPreviewModel value)
            : base(value)
        {
        }
    }
}
