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
    using Microsoft.Maui.Platform;

    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static Activity CurrentActivity;

        public MainActivity()
        {
            CurrentActivity = this;
        }

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        //public override bool DispatchTouchEvent(MotionEvent ev)
        //{
        //    if (ev.Action == MotionEventActions.Down ||
        //        ev.Action == MotionEventActions.Move ||
        //        ev.Action == MotionEventActions.Up)
        //    {
        //        byte count = 0;
        //        foreach (var item in (Window.DecorView as ViewGroup).GetChildrenOfType<EditText>())
        //        {
        //            if (!IsInsideEditViewTouch(item, ev.RawX, ev.RawY))
        //            {
        //                count++;
        //                break;
        //            }
        //        }

        //        if (count > 0)
        //        {
        //            var currentFocus = Platform.CurrentActivity.CurrentFocus;
        //            if (currentFocus != null)
        //            {
        //                Platform.CurrentActivity.HideKeyboard(currentFocus);
        //            }
        //        }
        //    }

        //    return base.DispatchTouchEvent(ev);
        //}

        private bool IsInsideEditViewTouch(EditText editText, float xCoordinateTouch, float yCoordinateTouch)
        {
            int[] position = new int[2];
            editText.GetLocationOnScreen(position);
            var editTextRect = new Rect(position[0], position[1], editText.Width, editText.Height);
            return editTextRect.Contains(xCoordinateTouch, yCoordinateTouch);
        }
    }
}