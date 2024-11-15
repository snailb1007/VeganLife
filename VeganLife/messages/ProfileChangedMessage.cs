// <copyright file="ProfileChangedMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VeganLife.Messages
{
    public class ProfileChangedMessage : ValueChangedMessage<object>
    {
        public ProfileChangedMessage(object? value)
            : base(value)
        {
        }
    }
}
