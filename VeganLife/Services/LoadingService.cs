using Mopups.Interfaces;
using Mopups.Services;
using VeganLife.Helpers;
using VeganLife.Views.Popups;

namespace VeganLife.Services
{
    public partial class LoadingService : ObservableObject, ILoadingService, IDisposable
    {
        private readonly IPopupNavigation _navigation;

        private ushort _delayTime;

        [ObservableProperty]
        private bool _isLoading;

        public LoadingService()
        {
            _navigation = MopupService.Instance;
        }

        public async void Dispose()
        {
            await _navigation.PopAsync();
            await Task.Delay(_delayTime).ContinueWith(t => IsLoading = false).ConfigureAwait(false);
        }

        public async Task<IDisposable> Show(ushort delayTime = 0)
        {
            IsLoading = true;
            _delayTime = delayTime;
            await _navigation.PushAsync(ServicesHelper.GetService<LoadingPopup>());
            return this;
        }
    }
}