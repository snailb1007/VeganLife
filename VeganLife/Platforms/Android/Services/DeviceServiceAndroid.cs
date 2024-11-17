// <copyright file="DeviceServiceAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Android.Content;
using Android.OS;
using Android.Views;
using VeganLife.Helpers.Extensions;

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
                var resources = Platform.CurrentActivity?.Resources;
                if (resources != null)
                {
                    return (int)resources.GetDimensionPixelSize(resourceId);
                }
            }

            return 0;
        }

        public string GetDeviceId()
        {
            var context = Android.App.Application.Context;
            var res = Android.Provider.Settings.Secure.GetString(context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            return res ?? string.Empty;
        }

        public bool IsAutomaticDateTimeEnabled()
        {
            return this.CheckIfSettingEnabled(global::Android.Provider.Settings.Global.AutoTime);
        }

        public bool IsAutomaticTimeZoneEnabled()
        {
            return this.CheckIfSettingEnabled(global::Android.Provider.Settings.Global.AutoTimeZone);
        }

        public void OpenDateSettings()
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionDateSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intent);
        }

        public void SetNavigationBarColor(string hexColor)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    if (OperatingSystem.IsAndroidVersionAtLeast(35))
                    {
                        // TODO: net9
                    }
                    else
                    {
                        currentWindow?.SetNavigationBarColor(global::Android.Graphics.Color.ParseColor(hexColor));
                    }
                });
            }
        }

        private Android.Views.Window GetCurrentWindow()
        {
            var window = Platform.CurrentActivity?.Window;
            if (window == null)
            {
                return null;
            }

            try
            {
                // clear FLAG_TRANSLUCENT_STATUS flag:
                window.ClearFlags(WindowManagerFlags.TranslucentStatus);

                // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

                return window;
            }
            catch
            {
                return null;
            }
        }

        private bool CheckIfSettingEnabled(string settingName)
        {
            try
            {
                if (Platform.CurrentActivity != null)
                {
                    return global::Android.Provider.Settings.Global.GetInt(Platform.CurrentActivity.ContentResolver,
                        settingName) == 1;
                }

                UtilitiesExtension.LogError("Platform.CurrentActivity is null");
                return false;

            }
            catch (global::Android.Provider.Settings.SettingNotFoundException e)
            {
                e.LogError(description: $"{nameof(DeviceService)} {settingName}");
                return false;
            }
            catch (Exception e)
            {
                UtilitiesExtension.LogError($"Checking setting {settingName}: {e.Message}");
                return false;
            }
        }
    }
}
