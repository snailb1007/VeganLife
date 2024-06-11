using Android.Content;
using Android.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using VeganLife.messages;
using VeganLife.Platforms.Android.Vision;
using VeganLife.Views.Controls;

namespace VeganLife.Handlers
{
    public class Camera2PreviewHandlerAndroid :
        ViewRenderer<Camera2PreviewView, RecognizerCamera2>,
        IRecipient<AppResumeMessage>
    {
        readonly Context _context;

        internal RecognizerCamera2 Camera2Droid { get; private set; }

        public Camera2PreviewHandlerAndroid(Context context)
            : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Camera2PreviewView> e)
        {
            base.OnElementChanged(e);
            Camera2Droid = new RecognizerCamera2(global::Android.App.Application.Context);
            if (e.OldElement != null)
            {
                return;
            }

            if (Control == null)
            {
                var inflater = LayoutInflater.FromContext(_context);
                if (inflater == null)
                {
                    return;
                }

                AddView(Camera2Droid.CameraTexturePreview);
                SetNativeControl(Camera2Droid);
            }
        }

        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            App.Window.Stopped -= Window_Stopped;
            WeakReferenceMessenger.Default.Unregister<AppResumeMessage>(this);
            if (Camera2Droid != null
                //&& Camera2Droid.RecognizeCompletionSource.Task != null
                )
            {
                //Camera2Droid.RecognizeCompletionSource.TrySetResult(new VinModel());
                Camera2Droid.DisposeControl();
            }
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            App.Window.Stopped += Window_Stopped;
            if (!WeakReferenceMessenger.Default.IsRegistered<AppResumeMessage>(this))
            {
                WeakReferenceMessenger.Default.Register(this);
            }
        }

        private void Window_Stopped(object sender, EventArgs e)
        {
            StopCamera();
        }

        public void Receive(AppResumeMessage message)
        {
            StartCamera();
        }

        private void StopCamera()
        {
            Camera2Droid.CloseCamera();
            Camera2Droid.StopBackgroundThread();
        }

        private void StartCamera()
        {
            //if (Camera2Droid.IsTimeOut)
            //{
            //    return;
            //}

            Camera2Droid.StartBackgroundThread();
            Camera2Droid.OpenCamera(0);
        }
    }
}
