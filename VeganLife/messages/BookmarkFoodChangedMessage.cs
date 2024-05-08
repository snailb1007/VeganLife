// <copyright file="BookmarkFoodChangedMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;
using VeganLife.Models.FoodModel;

namespace VeganLife.Messages
{
    public class BookmarkFoodChangedMessage : ValueChangedMessage<FoodPreviewModel>
    {
        public BookmarkFoodChangedMessage(FoodPreviewModel value)
            : base(value)
        {
        }
    }
}
