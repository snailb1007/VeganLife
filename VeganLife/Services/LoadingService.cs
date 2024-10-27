using AsyncAwaitBestPractices;
using Mopups.Interfaces;
using Mopups.Services;
using System.Reactive.Subjects;
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

        public void Dispose()
        {
            _navigation.PopAsync().SafeFireAndForget();
            _ = Task.Delay(_delayTime)
                .ContinueWith(t => IsLoading = false);
        }

        public async Task<IDisposable> Show(ushort delayTime = 0)
        {
            IsLoading = true;
            _delayTime = delayTime;
            await _navigation.PushAsync(ServicesHelper.GetService<LoadingPopup>());
            return this;
        }

        private int _requestCount;
        private bool _active = true;
        public bool Active
        {
            get => _active;

            set
            {
                if (!value)
                {
                    _active = value;
                    _requestCount = 0;
                    _whenRequestCountChanged.OnNext(0);
                }

                _active = value;
            }
        }

        private readonly ISubject<int> _whenRequestCountChanged = new Subject<int>();

        //public ViewModelBusyManager(Action<bool> setBusyFunc, bool isLoadingIconAppearing)
        //{
        //    Active = isLoadingIconAppearing;
        //    _whenRequestCountChanged
        //        .Select(count => count > 0)
        //        .ObserveOn(SynchronizationContext.Current)
        //        .Subscribe(busy =>
        //        {
        //            setBusyFunc(busy);
        //        });
        //}

        public void IncreaseRequest()
        {
            if (Active)
                _whenRequestCountChanged.OnNext(++_requestCount);
        }

        public void DecreaseRequest()
        {
            if (_requestCount > 0 && Active)
                _whenRequestCountChanged.OnNext(--_requestCount);
        }
    }
}