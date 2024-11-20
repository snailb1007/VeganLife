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
        /// <inheritdoc/>
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new ShellItemHandlerAndroid(this);
        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return base.CreateBottomNavViewAppearanceTracker(shellItem);
        }
    }

    public class ShellBottomNaviHandler : ShellBottomNavViewAppearanceTracker
    {
        const byte _messageTabIndex = 1;

        BottomNavigationView _bottomNaviView;
        BadgeDrawable _messBadge;
        int? _messTabId => _bottomNaviView?.Menu?.FindItem(_messageTabIndex)?.ItemId;

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
                    _messBadge.ClearNumber();
                    if (_messTabId is not null)
                        _bottomNaviView.RemoveBadge(_messTabId.Value);
                    _messBadge = null;
                }
                else
                {
                    var messNavi = _bottomNaviView?.Menu?.FindItem(_messageTabIndex);
                    var widthMess = messNavi?.Icon?.IntrinsicWidth;
                    if (_messTabId is not null)
                    {
                        _messBadge = _bottomNaviView.GetOrCreateBadge(_messTabId.Value);
                        _messBadge.VerticalOffset = 10;
                        _messBadge.HorizontalOffset = (int)Math.Abs((decimal)(widthMess * 0.3));
                        _messBadge.BadgeTextColor = global::Android.Graphics.Color.White;
                        _messBadge.BackgroundColor = global::Android.Graphics.Color.Red;
                        _messBadge.Number = number;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
