using CommunityToolkit.Mvvm.Messaging;
using VeganLife.Data.LocalData;
using VeganLife.Messages;
using VeganLife.Models.FoodModel;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel, IRecipient<BookmarkFoodModelMessage>
    {
        FoodPreviewDataStoreService _dataStoreService;
        IEnumerable<FoodPreviewModel> _onlineFoodPreviewData;
        IList<FoodPreviewModel> _allFoods = new List<FoodPreviewModel>();

        [ObservableProperty]
        ObservableCollection<FoodPreviewModel> _foods;
        [ObservableProperty]
        IEnumerable<FoodMenuCategoryModel> _category;
        [ObservableProperty]
        FoodPreviewModel _currentFoodSelected;

        public MainViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            _dataStoreService = new FoodPreviewDataStoreService(local_database);
            Init();
            WeakReferenceMessenger.Default.Register<BookmarkFoodModelMessage>(this);
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

                    (await _dataStoreService.GetItemsAsync()).ToList().ForEach(i => _allFoods.Add(i));
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
                        _allFoods.Add(i);
                        await _dataStoreService.AddOrUpdateItemAsync(i);
                    });
                }
            }

            foreach (var i in _allFoods)
                Foods.Add(i);
            await SetupMenu();
        }

        async Task SetupMenu()
        {
            Category = await data_service.GetFoodMenu();
        }

        [RelayCommand]
        async Task GoFoodDetail(object obj)
        {
            await navigation_service.NavigateToFoodDetail(obj);
            CurrentFoodSelected = null;
        }


        [RelayCommand]
        async Task CategoryClicked(object obj)
        {
            var itemMenu = obj as FoodMenuCategoryModel;
            if (itemMenu == null) return;
            var foodByCategory = _allFoods.Where(i => i.Category.Contains(itemMenu.Title));
            await Console.Out.WriteLineAsync("==>CategoryClicked" + foodByCategory.Count());
            var consignment = new Dictionary<string, IEnumerable<FoodPreviewModel>>();
            consignment.Add(itemMenu.Category, foodByCategory);
            await navigation_service.NavigateToCategoryPage(consignment);
        }

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            return base.OnNavigatedFrom(isForwardNavigation);
        }

        public async void Receive(BookmarkFoodModelMessage message)
        {
            if (message == null)
                return;
            message.Value.IsBookmarked = !message.Value.IsBookmarked;
            if (!await _dataStoreService.AddOrUpdateItemAsync(message.Value, true))
                await navigation_service.DisplayAlert("Error", "Oh, lỗi rồi!", "ok");
            WeakReferenceMessenger.Default.Send(new BookmarkFoodChangedMessage(message.Value));
        }
    }

    public class FoodMenuCategoryModel : MenuModel
    {
        public string Category
        {
            get
            {
                switch (Title)
                {
                    case "breakfast":
                        return "Bữa sáng";
                    case "dessert":
                        return "Tráng miệng";
                    case "dinner":
                        return "Bữa tối";
                    case "drink":
                        return "Đồ uống";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
