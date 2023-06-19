namespace VeganLife.ViewModels
{
    using CommunityToolkit.Mvvm.Messaging;
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Messages;
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.LocalDataServices;
    using VeganLife.Views.FoodTab;

    public partial class BookmarkViewModel : BaseViewModel, IRecipient<BookmarkFoodChangedMessage>
    {
        readonly FoodPreviewDataStoreService dataStoreService;

        [ObservableProperty]
        private ObservableCollection<FoodPreviewModel> foods;
        [ObservableProperty]
        private FoodPreviewModel foodSelected;

        public BookmarkViewModel()
            : base()
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
            {
                this.dataStoreService = new FoodPreviewDataStoreService(database);
            }

            this.Init();
            WeakReferenceMessenger.Default.Register<BookmarkFoodChangedMessage>(this);
        }

        private async void Init()
        {
            this.Foods = new ObservableCollection<FoodPreviewModel>();
            await this.LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            (await this.dataStoreService.GetItemsAsync())
                .Where(i => i.IsBookmarked)
                .ToList().ForEach(i => this.Foods.Add(i));
        }

        [RelayCommand]
        private async Task GoFoodDetail(object obj)
        {
            await this.dataStoreService.AddOrUpdateItemAsync(this.FoodSelected, true);
            await this.navigationService.NavigataToPage<FoodDetailPage>(obj);
        }

        /// <inheritdoc/>
        public override Task OnNavigatedFrom(bool isForwardNavigation)
        {
            this.FoodSelected = null;
            return base.OnNavigatedFrom(isForwardNavigation);
        }

        public void Receive(BookmarkFoodChangedMessage message)
        {
            if (message == null)
            {
                return;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (message.Value.IsBookmarked)
                {
                    this.Foods.Add(message.Value);
                }
                else
                {
                    this.Foods.ToList().ForEach(i =>
                    {
                        if (i.Id.Equals(message.Value.Id))
                        {
                            this.Foods.Remove(i);
                        }
                    });
                }
            });
        }

        // partial void OnFoodsChanged(ObservableCollection<FoodPreviewModel> value)
        // {
        //    Foods.ToList().ForEach(i =>
        //    {
        //        if (!i.IsBookmarked)
        //            Foods.Remove(i);
        //    });
        // }
    }
}
