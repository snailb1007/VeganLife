using Android.Hardware.Camera2;

namespace VeganLife.Platforms.Android.Vision
{
    internal class CameraStateListener : CameraDevice.StateCallback
    {
        public RecognizerCamera2 Recognizer { private get; set; }

        public CameraStateListener(RecognizerCamera2 camera2)
        {
            Recognizer = camera2;
        }

        public override void OnOpened(CameraDevice camera)
        {
            if (Recognizer == null)
                return;
            Recognizer.CameraOpenCloseLock.Release();
            Recognizer.CameraDevice = camera;
            Recognizer.StartPreview();
        }

        public override void OnDisconnected(CameraDevice camera)
        {
            if (Recognizer == null)
                return;
            Recognizer.CameraOpenCloseLock.Release();
            Recognizer.CloseDevice();
            Recognizer = null;
        }

        public override void OnError(CameraDevice camera, CameraError error)
        {
            camera.Close();
            if (Recognizer == null)
                return;
            Recognizer.CameraOpenCloseLock.Release();
            Recognizer.CloseDevice();
            Recognizer = null;
        }
    }
}
