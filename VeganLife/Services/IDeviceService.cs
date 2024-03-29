namespace VeganLife.Services
{
    public interface IDeviceService
    {
        void HideKeyboard();
        int GetDeviceDPI();
        string GetDeviceId();
        bool IsAutomaticDateTimeEnabled();
        bool IsAutomaticTimeZoneEnabled();
        void OpenDateSettings();
    }
}
