// <copyright file="MainActivity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Views;
    using Android.Widget;

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

        //private bool IsInsideEditViewTouch(EditText editText, float xCoordinateTouch, float yCoordinateTouch)
        //{
        //    int[] position = new int[2];
        //    editText.GetLocationOnScreen(position);
        //    var editTextRect = new Rect(position[0], position[1], editText.Width, editText.Height);
        //    return editTextRect.Contains(xCoordinateTouch, yCoordinateTouch);
        //}
    }
}