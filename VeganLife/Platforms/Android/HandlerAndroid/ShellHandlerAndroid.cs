// <copyright file="ShellHandlerAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Google.Android.Material.Badge;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform.Compatibility;
using VeganLife.Platforms.Android.HandlerAndroid;

namespace VeganLife.Handlers
{
    public partial class ShellHandler
    {
        ShellBottomNaviHandler _shellBottomNaviHandler;
        /// <inheritdoc/>
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new ShellItemHandlerAndroid(this);
        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            this._shellBottomNaviHandler = new ShellBottomNaviHandler(this, shellItem);
            return _shellBottomNaviHandler;
        }

        public void ChangeBageInfo(int infoNumber)
        {
            this._shellBottomNaviHandler.SetMessBadge(infoNumber);
        }
    }

    public class ShellBottomNaviHandler : ShellBottomNavViewAppearanceTracker
    {
        const byte _mealLogsTabIndex = 3;

        BottomNavigationView _bottomNaviView;
        BadgeDrawable _mealLogsTabBadge;
        int? _messTabId => _bottomNaviView?.Menu?.FindItem(_mealLogsTabIndex)?.ItemId;

        public ShellBottomNaviHandler(IShellContext shellContext, ShellItem shellItem)
            : base(shellContext, shellItem)
        {
        }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            _bottomNaviView = bottomView;
            _bottomNaviView.SetItemTextAppearanceActiveBoldEnabled(false);
        }

        public void SetMessBadge(int number)
        {
            try
            {
                if (number == 0)
                {
                    _mealLogsTabBadge.ClearNumber();
                    if (_messTabId is not null)
                        _bottomNaviView.RemoveBadge(_messTabId.Value);
                    _mealLogsTabBadge = null;
                }
                else
                {
                    var messNavi = _bottomNaviView?.Menu?.FindItem(_mealLogsTabIndex);
                    var widthMess = messNavi?.Icon?.IntrinsicWidth;
                    if (_messTabId is not null)
                    {
                        _mealLogsTabBadge = _bottomNaviView.GetOrCreateBadge(_messTabId.Value);
                        _mealLogsTabBadge.VerticalOffset = 10;
                        _mealLogsTabBadge.HorizontalOffset = (int)Math.Abs((decimal)(widthMess * 0.3));
                        _mealLogsTabBadge.BadgeTextColor = global::Android.Graphics.Color.White;
                        _mealLogsTabBadge.BackgroundColor = global::Android.Graphics.Color.Red;
                        _mealLogsTabBadge.Number = number;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
