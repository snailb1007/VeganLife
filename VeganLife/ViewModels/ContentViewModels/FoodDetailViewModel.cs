// <copyright file="FoodDetailViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.LocalDataServices;

    public partial class FoodDetailViewModel : BaseViewModel
    {
        FoodDetailDataStoreService _foodDetailDataStoreService;
        [ObservableProperty]
        FoodPreviewModel _foodPreview;

        [ObservableProperty]
        FoodDetailModel _foodDetail;

        public FoodDetailViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
                _foodDetailDataStoreService = new FoodDetailDataStoreService(database);
        }

        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter is not null)
            {
                FoodPreview = parameter as FoodPreviewModel;
                FoodPreview.IsRead = true;

                if (IsNetworkConnected)
                    FoodDetail = await dataService.GetFoodDetail(FoodPreview?.Id ?? string.Empty);
                if (FoodDetail == null)
                    FoodDetail = (await _foodDetailDataStoreService.GetItemsAsync()).FirstOrDefault();
                else
                    await _foodDetailDataStoreService.AddOrUpdateItemAsync(FoodDetail);
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        void OnBookmarkClicked()
        {
            if (FoodPreview == null) return;
            FoodPreview.BookmarkClickedCommand.Execute(null);
        }
    }
}
