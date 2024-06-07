using Android.Gms.Tasks;
using Xamarin.Google.MLKit.Vision.Pose;

namespace VeganLife.Platforms.Android.Vision
{
    internal class MLKitEventHandler : Java.Lang.Object, IOnSuccessListener, IOnFailureListener
    {
        public void OnFailure(Java.Lang.Exception e)
        {
            Debug.WriteLine(e.Message);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var res = (Pose)result;
        }
    }
}
