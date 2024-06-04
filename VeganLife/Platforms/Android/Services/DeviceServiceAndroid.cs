// <copyright file="DeviceServiceAndroid.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Android.Content;
using Android.OS;
using Android.Views;
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
                return (int)Platform.CurrentActivity?.Resources?.GetDimensionPixelSize(resourceId);
            }

            return 0;
        }

        public string GetDeviceId() => UUID.RandomUUID()?.ToString() ?? string.Empty;

        public bool IsAutomaticDateTimeEnabled()
        {
            try
            {
                if (Platform.CurrentActivity is null)
                {
#if DEBUG
                    Console.WriteLine("==> Platform.CurrentActivity is null");
#endif
                    return false;
                }

                return global::Android.Provider.Settings.Global.GetInt(Platform.CurrentActivity.ContentResolver, global::Android.Provider.Settings.Global.AutoTime) == 1;
            }
            catch (Exception e)
            {
                _ = e;
                if (e is global::Android.Provider.Settings.SettingNotFoundException nativeEx)
                {
                    _ = nativeEx;
#if DEBUG
                    Console.WriteLine($"==> {nameof(DeviceService)} IsAutomaticDateTimeEnabled:\n{nativeEx.Message}");
#endif
                }

                return false;
            }
        }

        public bool IsAutomaticTimeZoneEnabled()
        {
            try
            {
                if (Platform.CurrentActivity is null)
                {
#if DEBUG
                    Console.WriteLine("==> Platform.CurrentActivity is null");
#endif
                    return false;
                }

                return global::Android.Provider.Settings.Global.GetInt(Platform.CurrentActivity.ContentResolver, global::Android.Provider.Settings.Global.AutoTimeZone) == 1;
            }
            catch (Exception e)
            {
                _ = e;
                if (e is global::Android.Provider.Settings.SettingNotFoundException nativeEx)
                {
                    _ = nativeEx;
#if DEBUG
                    Console.WriteLine($"==> {nameof(DeviceService)} IsAutomaticDateTimeEnabled:\n{nativeEx.Message}");
#endif
                }

                return false;
            }
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
                    currentWindow?.SetNavigationBarColor(global::Android.Graphics.Color.ParseColor(hexColor));
                });
            }
        }

        private Android.Views.Window? GetCurrentWindow()
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
    }
}
