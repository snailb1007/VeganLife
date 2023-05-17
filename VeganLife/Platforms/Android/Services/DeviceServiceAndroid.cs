namespace VeganLife.Services
{
    public partial class DeviceService
    {
        const string naviBar = "navigation_bar_height";
        const string statusBar = "status_bar_height";
        public int GetDeviceDPI()
        {
            int resourceId = MainActivity.CurrentActivity.Resources.GetIdentifier(naviBar, "dimen", "android");
            if (resourceId > 0)
                return MainActivity.CurrentActivity.Resources.GetDimensionPixelSize(resourceId);
            return 0;
        }
    }
}
