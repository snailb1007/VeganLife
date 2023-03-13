using VeganLife.Data.LocalData;
using VeganLife.Helpers;
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

        public IList<MenuModel> Categorys = new List<MenuModel>()
        {
            new MenuModel() { Title = "Bữa sáng" },
            new MenuModel() { Title = "Đồ uống" },
            new MenuModel() { Title = "Bữa tối" },
            new MenuModel() { Title = "Món tráng miệng" },
        };

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
            //var menu = await data_service.GetFoodMenu();
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
        }

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            Console.WriteLine("==>OnNavigatedFrom");
            return base.OnNavigatedFrom(isForwardNavigation);
        }
    }
}
