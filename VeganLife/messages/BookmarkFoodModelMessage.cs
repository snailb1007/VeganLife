// <copyright file="BookmarkFoodModelMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;
using VeganLife.Models.FoodModel;

namespace VeganLife.Messages
{
    public class BookmarkFoodModelMessage : ValueChangedMessage<FoodPreviewModel>
    {
        public BookmarkFoodModelMessage(FoodPreviewModel value)
            : base(value)
        {
        }
    }
}
