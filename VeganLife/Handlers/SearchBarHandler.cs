using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace VeganLife.Handlers
{
    internal class SearchBarHandler : Microsoft.Maui.Handlers.SearchBarHandler
    {
        protected override void ConnectHandler(AndroidX.AppCompat.Widget.SearchView platformView)
        {
            LinearLayout? linearLayout = platformView.GetChildAt(0) as LinearLayout;
            linearLayout = linearLayout?.GetChildAt(2) as LinearLayout;
            linearLayout = linearLayout?.GetChildAt(1) as LinearLayout;
            if (linearLayout != null)
            {
                linearLayout.Background = null;
                platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());
            }

            base.ConnectHandler(platformView);
        }
    }
}
