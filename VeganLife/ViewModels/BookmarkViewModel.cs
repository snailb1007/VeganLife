using CommunityToolkit.Mvvm.Messaging;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Messages;
using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;
using VeganLife.Views.FoodTab;

namespace VeganLife.ViewModels
{
    public partial class BookmarkViewModel : BaseViewModel, IRecipient<BookmarkFoodChangedMessage>
    {
        FoodPreviewDataStoreService _dataStoreService;

        [ObservableProperty]
        ObservableCollection<FoodPreviewModel> _foods;
        [ObservableProperty]
        FoodPreviewModel _foodSelected;
        public BookmarkViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
                _dataStoreService = new FoodPreviewDataStoreService(database);
            Init();
            WeakReferenceMessenger.Default.Register<BookmarkFoodChangedMessage>(this);
        }

        async void Init()
        {
            Foods = new ObservableCollection<FoodPreviewModel>();
            await LoadDataAsync();
        }

        async Task LoadDataAsync()
        {
            (await _dataStoreService.GetItemsAsync())
                .Where(i => i.IsBookmarked)
                .ToList().ForEach(i => Foods.Add(i));
        }

        [RelayCommand]
        async Task GoFoodDetail(object obj)
        {
            await _dataStoreService.AddOrUpdateItemAsync(FoodSelected, true);
            await navigation_service.NavigataToPage<FoodDetailPage>(obj);
        }

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            FoodSelected = null;
            return base.OnNavigatedFrom(isForwardNavigation);
        }

        public override Task OnNavigatedTo()
        {
            return base.OnNavigatedTo();
        }

        public void Receive(BookmarkFoodChangedMessage message)
        {
            if (message == null)
                return;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (message.Value.IsBookmarked)
                {
                    Foods.Add(message.Value);
                }
                else
                {
                    Foods.ToList().ForEach(i =>
                    {
                        if (i.Id.Equals(message.Value.Id))
                            Foods.Remove(i);
                    });
                }
            });
        }

        //partial void OnFoodsChanged(ObservableCollection<FoodPreviewModel> value)
        //{
        //    Foods.ToList().ForEach(i =>
        //    {
        //        if (!i.IsBookmarked)
        //            Foods.Remove(i);
        //    });
        //}
    }
}
