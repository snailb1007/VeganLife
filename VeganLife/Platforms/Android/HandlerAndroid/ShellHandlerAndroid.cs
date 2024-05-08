// <copyright file="ShellHandlerAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Maui.Controls.Platform.Compatibility;
using VeganLife.Platforms.Android.HandlerAndroid;

namespace VeganLife.Handlers
{
    public partial class ShellHandler
    {
        /// <inheritdoc/>
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new ShellItemHandlerAndroid(this);
        }
    }
}
