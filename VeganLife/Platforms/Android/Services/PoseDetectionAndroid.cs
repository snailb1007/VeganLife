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
    }
}
