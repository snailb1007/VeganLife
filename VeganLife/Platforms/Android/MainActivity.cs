// <copyright file="MainActivity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Plugin.MauiMTAdmob;

namespace VeganLife
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public MainActivity()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var appId = "ca-app-pub-4076544648724623~8681233278";
            CrossMauiMTAdmob.Current.Init(this, appId);
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            return e?.PointerCount == 1 && base.DispatchTouchEvent(e);
        }
    }
}