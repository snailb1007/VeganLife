// <copyright file="BmiResultSelectedOptionMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace VeganLife.messages
{
    public class BmiResultSelectedOptionMessage : ValueChangedMessage<byte>
    {
        public BmiResultSelectedOptionMessage(byte value)
            : base(value)
        {
        }
    }
}
