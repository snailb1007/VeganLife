// <copyright file="DeviceServiceAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Java.Util;

namespace VeganLife.Services
{
    public partial class DeviceService
    {
        private const string NaviBar = "navigation_bar_height";

        public int GetDeviceDPI()
        {
            int resourceId = Platform.CurrentActivity?.Resources?.GetIdentifier(NaviBar, "dimen", "android") ?? 0;
            if (resourceId > 0)
            {
                return (int)(Platform.CurrentActivity?.Resources?.GetDimensionPixelSize(resourceId))!;
            }

            return 0;
        }

        public string GetDeviceId() => UUID.RandomUUID()?.ToString() ?? string.Empty;
    }
}
