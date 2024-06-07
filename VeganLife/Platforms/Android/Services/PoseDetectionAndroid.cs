using Android.Gms.Extensions;
using Android.Graphics;
using VeganLife.Platforms.Android.Vision;
using Xamarin.Google.MLKit.Vision.Common;
using Xamarin.Google.MLKit.Vision.Pose;
using Xamarin.Google.MLKit.Vision.Pose.Accurate;
using Xamarin.Google.MLKit.Vision.Pose.Defaults;

namespace VeganLife.Platforms.Android.Services
{
    internal class PoseDetectionAndroid
    {
        private IPoseDetector _poseDetector;

        public PoseDetectionAndroid()
        {
            var builder = new PoseDetectorOptions.Builder();
            builder.SetDetectorMode(AccuratePoseDetectorOptions.StreamMode);
            var poseDetector = builder.Build();
            _poseDetector = PoseDetection.GetClient(poseDetector);
        }

        // detect pose
        public async Task DetectPoseAsync(Bitmap rawImg)
        {
            using (var callBack = new MLKitEventHandler())
            using (var image = InputImage.FromBitmap(rawImg, 0))
            {
                var pose = await _poseDetector.Process(image)
                    .AddOnSuccessListener(callBack)
                    .AddOnFailureListener(callBack);
            }
        }
    }
}