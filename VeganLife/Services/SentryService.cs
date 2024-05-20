using VeganLife.Helpers.AppSetting;

namespace VeganLife.Services
{
    internal class SentryService
    {
        private bool _isEnabled;
        private IDisposable _sentryInstance;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (value != _isEnabled)
                {
                    if (value)
                    {
                        EnableSentry();
                    }
                    else
                    {
                        DisableSentry();
                    }
                }
            }
        }

        public SentryService(bool initialStatus = true)
        {
            _isEnabled = initialStatus;
            if (_isEnabled)
            {
                EnableSentry();
            }
        }

        private void EnableSentry()
        {
            if (_sentryInstance != null)
            {
                return;
            }

            _sentryInstance = SentrySdk.Init(options =>
            {
                options.Dsn = ConstantHelper.SentryConstant.SentryDsn;
                options.TracesSampleRate = 1.0;

                // options.Debug = true; // Set to false in production
            });
            _isEnabled = true;
        }

        private void DisableSentry()
        {
            if (_sentryInstance != null)
            {
                _sentryInstance.Dispose(); // Properly shutdown Sentry
            }

            _isEnabled = false;
        }

        public void CaptureException(Exception ex)
        {
            if (_isEnabled)
            {
                SentrySdk.CaptureException(ex);
            }
        }

        public void LogMessage(string message, SentryLevel level = SentryLevel.Info)
        {
            if (_isEnabled)
            {
                SentrySdk.CaptureMessage(message, level);
            }
        }

        public void AddBreadcrumb(string message, string category, string type, IDictionary<string, string> data = null)
        {
            if (_isEnabled)
            {
                SentrySdk.AddBreadcrumb(message, category, type, data);
            }
        }
    }
}
