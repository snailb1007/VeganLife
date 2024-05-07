// <copyright file="MainViewModel.cs" company="VeganLife">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels
{
    using System.Text.RegularExpressions;
    using CommunityToolkit.Mvvm.Messaging;
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Messages;
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.UserServices;
    using VeganLife.Views.MainPageFlyout.FoodTab;

    /// <summary>
    /// vm for MainPage.
    /// </summary>
    public partial class MainViewModel : BaseViewModel, IRecipient<BookmarkFoodModelMessage>
    {
        private FoodPreviewDataStoreService _dataStoreService;
        private IEnumerable<FoodPreviewModel> _onlineFoodPreviewData;
        private IList<FoodPreviewModel> _allFoods;

        // private IList<FoodPreviewModel> passFilterFoods;
        [ObservableProperty]
        private bool isSearchFocused;
        [ObservableProperty]
        private bool isFilterContentExpaned;
        [ObservableProperty]
        private ObservableCollection<FoodPreviewModel> foods;
        [ObservableProperty]
        private IEnumerable<FoodMenuCategoryModel> category;
        [ObservableProperty]
        private FoodPreviewModel currentFoodSelected;
        [ObservableProperty]
        private string searchText;

        private bool isLoadDataOnAppearingDone;

        // public IAsyncRelayCommand GoFoodDetailCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
            : base()
        {
            // this.GoFoodDetailCommand = new AsyncRelayCommand<object>(this.GoFoodDetail);
            this._allFoods = new List<FoodPreviewModel>();
            this.Init();
            WeakReferenceMessenger.Default.Register<BookmarkFoodModelMessage>(this);
        }

        private void Init()
        {
            this.Foods = new ObservableCollection<FoodPreviewModel>();
            _dataStoreService = ServicesHelper.GetService<FoodPreviewDataStoreService>();
        }

        public override Task ViewAppearingVM()
        {
            if (!isLoadDataOnAppearingDone)
            {
                LoadDataCommand.Execute(null);
            }

            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            this.IsLoading = true;
            if (this.IsNetworkConnected)
            {
                this._onlineFoodPreviewData = await this.dataService.GetFoods();
            }

            this._allFoods.Clear();
            Foods.Clear();
            var localData = await _dataStoreService.GetItemsAsync();
            if (localData?.Any() ?? false)
            {
                if (this._onlineFoodPreviewData?.Any() ?? false)
                {
                    // compare local vs online => update
                    for (int i = 0; i < this._onlineFoodPreviewData.Count(); i++)
                    {
                        var thisOnlineItem = this._onlineFoodPreviewData.ElementAt(i);
                        bool thisItemAlreadyExisted = false;
                        foreach (var item in localData)
                        {
                            if (thisOnlineItem?.Id.Equals(item.Id) ?? false)
                            {
                                thisItemAlreadyExisted = true;
                                thisOnlineItem.IsBookmarked = item.IsBookmarked;
                                thisOnlineItem.IsRead = item.IsRead;
                                await this._dataStoreService.AddOrUpdateItemAsync(thisOnlineItem, true);
                            }
                        }

                        // add new item from server to local
                        if (!thisItemAlreadyExisted)
                        {
                            await this._dataStoreService.AddOrUpdateItemAsync(thisOnlineItem!);
                        }
                    }

                    (await this._dataStoreService.GetItemsAsync()).ToList().ForEach(i => this._allFoods.Add(i));
                }
                else
                {
                    localData.ToList().ForEach(i => this.Foods.Add(i));
                }
            }
            else
            {
                if (this._onlineFoodPreviewData?.Any() ?? false)
                {
                    this._onlineFoodPreviewData.ToList().ForEach(async i =>
                    {
                        this._allFoods.Add(i);
                        await _dataStoreService.AddOrUpdateItemAsync(i);
                    });
                }
            }

            foreach (var i in this._allFoods.Where(i => !string.IsNullOrEmpty(i.Name)))
            {
                this.Foods.Add(i);
            }

            await this.SetupMenu();
            this.isLoadDataOnAppearingDone = true;
            IsLoading = false;
        }

        private async Task SetupMenu()
        {
            this.Category = await this.dataService.GetFoodMenu();
        }

        [RelayCommand]
        private async Task GoFoodDetail(object obj)
        {
            if (this.GoFoodDetailCommand.IsRunning)
            {
                return;
            }

            IsLoading = true;
            await this.navigationService.NavigateToPage<FoodDetailPage>(obj);
            this.CurrentFoodSelected = null!;
            var userService = ServicesHelper.GetService<IUserDataService>();
            await userService.Refresh();
            var userInfo = (userService as UserDataService)?.UserInfo!;
            if (userInfo != null)
            {
                userInfo.TotalFoodDetailRead++;
            }

            await userService.SaveData();
            IsLoading = false;
        }

        [RelayCommand]
        private async Task CategoryClicked(object obj)
        {
            if (this._allFoods == null || !this._allFoods.Any())
            {
                return;
            }

            var itemMenu = obj as FoodMenuCategoryModel;
            if (itemMenu == null)
            {
                return;
            }

            IsLoading = true;
            var foodByCategory = this._allFoods.Where(i => i.Category.Contains(itemMenu.Title))
                .ToList();
            var consignment = new Dictionary<string, IEnumerable<FoodPreviewModel>>
            {
                { itemMenu.Category, foodByCategory }
            };
            await this.navigationService.NavigateToPage<FoodsByCategoryPage>(consignment);
            IsLoading = false;
        }

        [RelayCommand]
        private void FilterClicked()
        {
            this.IsFilterContentExpaned = !this.IsFilterContentExpaned;
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            Foods = new ObservableCollection<FoodPreviewModel>(this.FilterKeySearch(this._allFoods.ToList(), this.SearchText));
        }

        /// <inheritdoc/>
        public void Receive(BookmarkFoodModelMessage message)
        {
            if (message == null)
            {
                return;
            }

            WeakReferenceMessenger.Default.Send(new BookmarkFoodChangedMessage(message.Value));
        }

        private IEnumerable<FoodPreviewModel> FilterKeySearch(List<FoodPreviewModel> foods, string key)
        {
            string[] words = Regex.Replace(key, @"\s+", " ").Split(' ');
            foreach (var item in foods)
            {
                var normalName = item.Name.ConvertStringToUnSigned() ?? string.Empty;
                int count = (from word in words
                             where normalName.Contains(word)
                             select word).Count();
                item.CountCorrectWordOnSearch = count;
            }

            var x = this.Foods.Where(w => w.CountCorrectWordOnSearch == words.Length).OrderByDescending(i => i.CountCorrectWordOnSearch);
            return x;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                Foods = new ObservableCollection<FoodPreviewModel>(this._allFoods);
            }
        }
    }
}
