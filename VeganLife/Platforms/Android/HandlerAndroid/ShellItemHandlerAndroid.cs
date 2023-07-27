// <copyright file="ShellItemHandlerAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Platforms.Android.HandlerAndroid
{
    using Microsoft.Maui.Controls.Platform.Compatibility;
    using VeganLife.Helpers;
    using VeganLife.Helpers.AppSetting;

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

        protected override bool OnItemSelected(global::Android.Views.IMenuItem item)
        {
            if (Shell.Current.IsBusy
                || (ServicesHelper.GetCurrentViewModel()?.IsLoading ?? false))
            {
                return false;
            }

            return base.OnItemSelected(item);
        }
    }
}
