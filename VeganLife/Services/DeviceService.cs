using Microsoft.Maui.Platform;

namespace VeganLife.Services
{
    public class DeviceService : IDeviceService
    {
        public void HideKeyboard()
        {
            if (Platform.CurrentActivity.CurrentFocus != null)
                Platform.CurrentActivity.HideKeyboard(Platform.CurrentActivity.CurrentFocus);
        }

        // public bool IsAndroid => DeviceInfo.Current.Platform == DevicePlatform.Android;

        // public bool IsVirtual = DeviceInfo.Current.DeviceType switch { DeviceType.Physical => false, DeviceType.Virtual => true, _ => false };

        // public double WidthScreen => Application.Current?.MainPage?.Width ?? default;
        // public double HeightScreen => Application.Current?.MainPage?.Height ?? default;
    }
}
