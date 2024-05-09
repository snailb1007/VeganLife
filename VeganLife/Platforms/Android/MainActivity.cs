// <copyright file="MainActivity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Android.App;
using Android.Content.PM;
using Android.Views;

namespace VeganLife
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public MainActivity()
        {
        }

        public override bool DispatchTouchEvent(MotionEvent? e)
        {
            return e?.PointerCount == 1 && base.DispatchTouchEvent(e);
        }
    }
}