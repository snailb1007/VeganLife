using CommunityToolkit.Mvvm.Messaging;
using System.Text.RegularExpressions;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Messages;
using VeganLife.Models.FoodModel;
using VeganLife.Views.FoodTab;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel, IRecipient<BookmarkFoodModelMessage>
    {
        public static FoodPreviewDataStoreService DataStoreService;
        IEnumerable<FoodPreviewModel> _onlineFoodPreviewData;
        IList<FoodPreviewModel> _allFoods = new List<FoodPreviewModel>();
        IEnumerable<FoodPreviewModel> _passFilterFoods;

        [ObservableProperty]
        bool _isSearchFocused;
        [ObservableProperty]
        bool _isFilterContentExpaned;
        [ObservableProperty]
        ObservableCollection<FoodPreviewModel> _foods;
        [ObservableProperty]
        IEnumerable<FoodMenuCategoryModel> _category;
        [ObservableProperty]
        FoodPreviewModel _currentFoodSelected;
        [ObservableProperty]
        string _searchText;

        public bool IsLoadDataOnAppearingDone { get; private set; }

        public IAsyncRelayCommand GoFoodDetailCommand { get; }
        public IAsyncRelayCommand LoadDataCommand { get; }

        public MainViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            GoFoodDetailCommand = new AsyncRelayCommand<object>(GoFoodDetail);
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            Init();
            WeakReferenceMessenger.Default.Register<BookmarkFoodModelMessage>(this);
        }

        void Init()
        {
            Foods = new ObservableCollection<FoodPreviewModel>();
            DataStoreService = new FoodPreviewDataStoreService(local_database);
        }

        async Task LoadDataAsync()
        {
            if (IsNetworkConnected)
            {
                _onlineFoodPreviewData = await data_service.GetFoods();
            }

            var localData = await DataStoreService.GetItemsAsync();
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
                                await DataStoreService.AddOrUpdateItemAsync(thisOnlineItem, true);
                            }
                        }
                        // add new item from server to local
                        if (!thisItemAlreadyExisted)
                        {
                            await DataStoreService.AddOrUpdateItemAsync(thisOnlineItem);
                        }
                    }

                    (await DataStoreService.GetItemsAsync()).ToList().ForEach(i => _allFoods.Add(i));
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
                        await DataStoreService.AddOrUpdateItemAsync(i);
                    });
                }
            }

            foreach (var i in _allFoods)
                Foods.Add(i);
            await SetupMenu();
            IsLoadDataOnAppearingDone = true;
        }

        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            return base.OnNavigatedFrom(isForwardNavigation);
        }

        async Task SetupMenu()
        {
            Category = await data_service.GetFoodMenu();
        }

        async Task GoFoodDetail(object obj)
        {
            if (GoFoodDetailCommand.IsRunning)
                return;
            await navigation_service.NavigataToPage<FoodDetailPage>(obj);
            CurrentFoodSelected = null;
        }


        [RelayCommand]
        async Task CategoryClicked(object obj)
        {
            var itemMenu = obj as FoodMenuCategoryModel;
            if (itemMenu == null) return;
            var foodByCategory = _allFoods.Where(i => i.Category.Contains(itemMenu.Title));
            var consignment = new Dictionary<string, IEnumerable<FoodPreviewModel>>();
            consignment.Add(itemMenu.Category, foodByCategory);
            await navigation_service.NavigataToPage<FoodsByCategoryPage>(consignment);
        }

        [RelayCommand]
        void FilterClicked()
        {
            IsFilterContentExpaned = !IsFilterContentExpaned;
        }

        [RelayCommand]
        void EnsureSearch()
        {
            if (string.IsNullOrEmpty(SearchText) || string.IsNullOrWhiteSpace(SearchText))
            {
                Foods.Clear();
                foreach (var i in _allFoods)
                    Foods.Add(i);
                return;
            }

            _passFilterFoods = FilterKeySearch(_allFoods.ToList(), SearchText);
            Foods.Clear();
            foreach (var i in _passFilterFoods)
            {
                Foods.Add(i);
            }
        }

        public void Receive(BookmarkFoodModelMessage message)
        {
            if (message == null)
                return;
            WeakReferenceMessenger.Default.Send(new BookmarkFoodChangedMessage(message.Value));
        }

        IEnumerable<FoodPreviewModel> FilterKeySearch(List<FoodPreviewModel> foods, string key)
        {
            string[] words = Regex.Replace(key, @"\s+", " ").Split(' ');
            foreach (var item in foods)
            {
                var normalName = item.Name.ConvertStringToUnSigned() ?? string.Empty;
                byte count = 0;
                foreach (var word in words)
                {
                    if (normalName.Contains(word))
                        count++;
                }

                item.CountCorrectWordOnSearch = count;
            }

            return foods.Where(w => w.CountCorrectWordOnSearch == words.Length).OrderByDescending(i => i.CountCorrectWordOnSearch);
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
