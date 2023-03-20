using CommunityToolkit.Mvvm.Messaging;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Messages;
using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        FoodPreviewDataStoreService _dataStoreService;
        IEnumerable<FoodPreviewModel> _onlineFoodPreviewData;

        [ObservableProperty]
        ObservableCollection<FoodPreviewModel> _foods;
        [ObservableProperty]
        IList<MenuModel> _category;

        public MainViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
                _dataStoreService = new FoodPreviewDataStoreService(database);
            Init();
        }

        async void Init()
        {
            Foods = new ObservableCollection<FoodPreviewModel>();
            if (AccessType == NetworkAccess.Internet)
                _onlineFoodPreviewData = await data_service.GetFoods();
            var localData = await _dataStoreService.GetItemsAsync();
            if (localData?.Any() ?? false)
            {
                if (_onlineFoodPreviewData?.Any() ?? false)
                {
                    // compare local vs online => update
                    for (int i = 0; i < _onlineFoodPreviewData.Count(); i++)
                    {
                        var thisOnlineItem = _onlineFoodPreviewData.ElementAt(i);
                        bool thisItemAlreadyExisted = false;
                        foreach (var item in localData)
                        {
                            if (thisOnlineItem?.Id.Equals(item.Id) ?? false)
                            {
                                thisItemAlreadyExisted = true;
                                thisOnlineItem.IsBookmarked = item.IsBookmarked;
                                thisOnlineItem.IsRead = item.IsRead;
                                await _dataStoreService.AddOrUpdateItemAsync(thisOnlineItem, true);
                            }
                        }
                        // add new item from server to local
                        if (!thisItemAlreadyExisted)
                        {
                            await _dataStoreService.AddOrUpdateItemAsync(thisOnlineItem);
                        }
                    }

                    (await _dataStoreService.GetItemsAsync()).ToList().ForEach(i => Foods.Add(i));
                }
                else
                {
                    localData.ToList().ForEach(i => Foods.Add(i));
                }
            }
            else
            {
                if (_onlineFoodPreviewData?.Any() ?? false)
                {
                    _onlineFoodPreviewData.ToList().ForEach(async i =>
                    {
                        Foods.Add(i);
                        await _dataStoreService.AddOrUpdateItemAsync(i);
                    });
                }
            }

            var menu = await data_service.GetFoodMenu();
            Category = new List<MenuModel>();
            foreach(var i in menu)
            {
                switch (i.Title)
                {
                    case "breakfast":
                        i.Title = "Bữa sáng";
                        break;
                    case "dessert":
                        i.Title = "Tráng miệng";
                        break;
                    case "dinner":
                        i.Title = "Bữa tối";
                        break;
                    case "drink":
                        i.Title = "Đồ uống";
                        break;
                }

                Category.Add(i);
            }
        }

        [RelayCommand]
        async Task GoFoodDetail(object obj)
        {
            await navigation_service.NavigateToFoodDetail(obj);
        }

        [RelayCommand]
        async Task BookmarkClicked(object obj)
        {
            if (obj == null)
                return;
            var food = obj as FoodPreviewModel;
            food.IsBookmarked = !food.IsBookmarked;
            if (!await _dataStoreService.AddOrUpdateItemAsync(food, true))
                await navigation_service.DisplayAlert("Error", "Oh, lỗi rồi!", "ok");
            WeakReferenceMessenger.Default.Send(new BookmarkFoodChangedMessage(food));
        }

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            Console.WriteLine("==>OnNavigatedFrom");
            return base.OnNavigatedFrom(isForwardNavigation);
        }
    }
}
