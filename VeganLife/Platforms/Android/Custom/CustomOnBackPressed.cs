using Android.Runtime;
using AndroidX.Activity;

namespace VeganLife.Platforms.Android.Custom
{
    public class CustomOnBackPressed : OnBackPressedCallback
    {
        public CustomOnBackPressed(bool enabled) : base(enabled)
        {
        }

        public override void HandleOnBackPressed()
        {
            //code that handles back button pressed
            Console.WriteLine("==> CustomOnBackPressed HandleOnBackPressed");
        }
    }
}
