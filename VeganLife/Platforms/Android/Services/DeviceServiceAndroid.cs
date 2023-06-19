// <copyright file="DeviceServiceAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    public partial class DeviceService
    {
        private const string NaviBar = "navigation_bar_height";

        // private const string statusBar = "status_bar_height";
        public int GetDeviceDPI()
        {
            int resourceId = MainActivity.CurrentActivity.Resources.GetIdentifier(NaviBar, "dimen", "android");
            if (resourceId > 0)
            {
                return MainActivity.CurrentActivity.Resources.GetDimensionPixelSize(resourceId);
            }

            return 0;
        }
    }
}
