// <copyright file="DeviceService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    using Microsoft.Maui.Platform;

    public partial class DeviceService : IDeviceService
    {
        public void HideKeyboard()
        {
            if (Platform.CurrentActivity.CurrentFocus != null)
            {
                Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
            }
        }

        // public bool IsVirtual = DeviceInfo.Current.DeviceType switch { DeviceType.Physical => false, DeviceType.Virtual => true, _ => false };

        // public double WidthScreen => Application.Current?.MainPage?.Width ?? default;
        // public double HeightScreen => Application.Current?.MainPage?.Height ?? default;
    }
}
