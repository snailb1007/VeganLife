// <copyright file="ShellItemHandlerAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Platforms.Android.HandlerAndroid
{
    using Microsoft.Maui.Controls.Platform.Compatibility;
    using VeganLife.Helpers;

    public class ShellItemHandlerAndroid : ShellItemRenderer
    {
        public ShellItemHandlerAndroid(IShellContext shellContext)
            : base(shellContext)
        {
        }

        /// <inheritdoc/>
        protected override void OnTabReselected(ShellSection shellSection)
        {
            base.OnTabReselected(shellSection);
            var navi = ServicesHelper.GetService<INavigationService>();
            if (navi.GetStackCount() == 1)
            {
                return;
            }

            navi.PopToRootAsync();
        }
    }
}
