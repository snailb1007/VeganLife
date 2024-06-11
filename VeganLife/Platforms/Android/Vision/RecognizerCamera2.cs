using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Java.Lang;

namespace VeganLife.Platforms.Android.Vision
{
    public class RecognizerCamera2 : FrameLayout, TextureView.ISurfaceTextureListener
    {
        private global::Android.Util.Size _previewSize;
        private CaptureRequest.Builder _requestBuilder;
        private bool _isFlashSupported;
        private bool _isClosingCam;
        private bool _isTorchOn;
        private CameraManager _cameraManager;
        private CameraCaptureSession _previewSession;
        private CaptureRequest _previewRequest;
        private Handler _backgroundHandler;
        private CameraStateListener _mStateListener;
        private Context _context;
        private HandlerThread _backgroundThread;
        private string _cameraId;

        public Java.Util.Concurrent.Semaphore CameraOpenCloseLock = new Java.Util.Concurrent.Semaphore(1);

        public CameraDevice CameraDevice { get; set; }

        public TextureView CameraTexturePreview { get; set; }

        public bool OpeningCamera { get; set; }

        public RecognizerCamera2(Context context)
            : base(context)
        {
            _context = context;
            _mStateListener = new CameraStateListener(this);
            CameraTexturePreview = new TextureView(context);
            CameraTexturePreview.SurfaceTextureListener = this;
        }

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            StartBackgroundThread();
            OpenCamera(0);
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            surface.Dispose();
            CloseDevice();
            return false;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            surface.SetDefaultBufferSize(width, height);
        }

        private bool _isProcessingFrame;

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            surface.Dispose();
        }

        internal void StartPreview()
        {
            if (CameraDevice == null || !CameraTexturePreview.IsAvailable || _previewSize == null)
            {
                Java.Lang.Thread.Sleep(100);
            }

            var texture = CameraTexturePreview.SurfaceTexture;
            if (texture == null)
            {
                return;
            }

            texture.SetDefaultBufferSize(_previewSize.Width, _previewSize.Height);
            var surface = new Surface(texture);
            texture.Dispose();
            _requestBuilder = CameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
            if (_requestBuilder == null)
            {
                return;
            }

            _requestBuilder.AddTarget(surface);
            SetupCaptureRequest();
            var sessionCallback = new CameraCaptureStateListener
            {
                OnConfigureFailedAction = session =>
                {
                    //this.Log("Camera capture failed");
                    session.Close();
                    _previewSession = null;
                },
                OnConfiguredAction = session =>
                {
                    if (CameraDevice == null)
                    {
                        return;
                    }

                    _previewSession = session;
                    UpdatePreview();
                },
            };
            if (OperatingSystem.IsAndroidVersionAtLeast(28))
            {
                CameraDevice.CreateCaptureSession(
                    new SessionConfiguration(sessionType: 0,
                        new List<OutputConfiguration> { new OutputConfiguration(surface) },
                        Context.MainExecutor,
                        cb: sessionCallback));
                if (OperatingSystem.IsAndroidVersionAtLeast(30))
                {
                    CameraDevice.CameraAudioRestriction = 0;
                }
            }
            else
            {
                CameraDevice.CreateCaptureSession(
                    [surface],
                    new CameraCaptureStateListener
                    {
                        OnConfigureFailedAction = session =>
                        {
                            System.Diagnostics.Debug.WriteLine("Camera capture failed");
                            session.Close();
                            _previewSession = null;
                        },
                        OnConfiguredAction = session =>
                        {
                            if (CameraDevice == null)
                                return;
                            _previewSession = session;
                            UpdatePreview();
                        },
                    },
                    null);
            }

            void SetupCaptureRequest()
            {
                _requestBuilder.Set(CaptureRequest.ControlMode, (int)ControlMode.Auto);
                CameraCharacteristics cameraCharacteristics = _cameraManager.GetCameraCharacteristics("0");
                if (cameraCharacteristics != null)
                {
                    _isFlashSupported = (bool)cameraCharacteristics.Get(CameraCharacteristics.FlashInfoAvailable);
                    if (cameraCharacteristics.Keys.Contains(CameraCharacteristics.ControlAeAvailableTargetFpsRanges))
                    {
                        _requestBuilder.Set(CaptureRequest.ControlAeTargetFpsRange, new global::Android.Util.Range(30, 30));
                    }

                    if (cameraCharacteristics.Keys.Contains(CameraCharacteristics.NoiseReductionAvailableNoiseReductionModes))
                    {
                        _requestBuilder.Set(CaptureRequest.NoiseReductionMode, (int)NoiseReductionMode.Fast);
                    }

                    if (cameraCharacteristics.Keys.Contains(CameraCharacteristics.ControlAeAvailableModes))
                    {
                        _requestBuilder.Set(CaptureRequest.ControlAeMode, (int)ControlAEMode.On);
                    }

                    if (cameraCharacteristics.Keys.Contains(CameraCharacteristics.ControlAvailableVideoStabilizationModes))
                    {
                        _requestBuilder.Set(CaptureRequest.ControlVideoStabilizationMode, (int)ControlVideoStabilizationMode.Off);
                    }
                }

                _requestBuilder.Set(CaptureRequest.ControlCaptureIntent, (int)ControlCaptureIntent.Preview);
                _requestBuilder.Set(CaptureRequest.JpegQuality, (sbyte)100);
            }
        }

        internal void CloseDevice()
        {
            try
            {
                CloseSession();
                if (CameraDevice != null)
                {
                    if (_requestBuilder != null)
                    {
                        _requestBuilder.Dispose();
                        _requestBuilder = null;
                    }

                    CloseCamera();
                    CameraDevice = null;
                }
            }
            catch (InterruptedException)
            {
                System.Diagnostics.Debug.WriteLine("Interrupted while trying to lock camera closing.");
            }
        }

        internal void CloseCamera()
        {
            _isClosingCam = true;
            if (CameraDevice != null && OpeningCamera)
            {
                _backgroundHandler?.RemoveCallbacksAndMessages(this);
                CameraOpenCloseLock.Acquire();
                CameraDevice.Close();
                OpeningCamera = false;
                CameraOpenCloseLock.Release();
            }

            _isClosingCam = false;
        }

        internal void StartBackgroundThread()
        {
            if (_backgroundThread == null)
            {
                CameraOpenCloseLock = new Java.Util.Concurrent.Semaphore(1);
                _backgroundThread = new HandlerThread("CameraPreview");
                _backgroundThread.Start();
                _backgroundHandler = new Handler(_backgroundThread.Looper);
            }
        }

        internal void OpenCamera(int cameraOptions) // 0: rear camera; 1: front camera
        {
            if (_context == null || OpeningCamera)
            {
                return;
            }

            if (_cameraManager == null)
            {
                _cameraManager = (CameraManager)_context.GetSystemService(Context.CameraService);
            }

            try
            {
                if (_cameraId == null)
                {
                    _cameraId = _cameraManager.GetCameraIdList()[cameraOptions];
                }

                if (_previewSize == null)
                {
                    var characteristics = _cameraManager.GetCameraCharacteristics(_cameraId);
                    using (var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap))
                    {
                        _previewSize = map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture)))[0];
                    }
                }

                if (ContextCompat.CheckSelfPermission(Context, global::Android.Manifest.Permission.Camera) == 0)
                {
                    _cameraManager.OpenCamera(_cameraId, _mStateListener, null);
                    OpeningCamera = true;
                }
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
                OpeningCamera = false;
            }
            catch (InterruptedException e)
            {
                throw new RuntimeException("Interrupted while trying to lock camera opening.", e);
            }
        }

        internal void StopBackgroundThread()
        {
            if (_isTorchOn)
            {
                _isTorchOn = false;
            }

            //if (App.CurrentViewModel is ScanVinTextViewModel viewModel && viewModel.IsTorch)
            //    viewModel.IsTorch = false;
            _backgroundHandler?.RemoveCallbacksAndMessages(this);
            _backgroundThread?.Interrupt();
            _backgroundThread?.QuitSafely();
            try
            {
                _backgroundThread?.Join();
                _backgroundThread = null;
                _backgroundHandler = null;
            }
            catch (InterruptedException e)
            {
                e.PrintStackTrace();
            }
        }

        internal void DisposeControl()
        {
            CameraTexturePreview?.Dispose();
            RemoveAllViews();
            Dispose();
        }

        private void UpdatePreview()
        {
            if (CameraDevice == null || _previewSession == null || !OpeningCamera)
            {
                return;
            }

            try
            {
                _previewRequest = _requestBuilder.Build();
                _previewSession.SetRepeatingRequest(_previewRequest, null, _backgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
        }

        private void CloseSession()
        {
            bool isRework = CameraOpenCloseLock.AvailablePermits() == 2;
            if (isRework)
            {
                CameraOpenCloseLock = new Java.Util.Concurrent.Semaphore(1);
            }

            _backgroundHandler?.PostDelayed(
                new Runnable(() =>
                {
                    if (CameraDevice == null || !OpeningCamera || _isClosingCam)
                    {
                        return;
                    }

                    _previewSession.StopRepeating();
                    _previewSession.AbortCaptures();
                    _previewSession.Close();
                    if (_previewSession != null)
                    {
                        _previewSession = null;
                    }
                }),
                isRework ? 1 : 0);
        }
    }
}
