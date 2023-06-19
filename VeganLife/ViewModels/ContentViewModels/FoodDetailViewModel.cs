// <copyright file="FoodDetailViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.LocalDataServices;

    /// <summary>
    /// vm for FoodDetailPage.
    /// </summary>
    public partial class FoodDetailViewModel : BaseViewModel
    {
        private FoodDetailDataStoreService foodDetailDataStoreService;
        [ObservableProperty]
        private FoodPreviewModel foodPreview;

        [ObservableProperty]
        private FoodDetailModel foodDetail;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoodDetailViewModel"/> class.
        /// </summary>
        public FoodDetailViewModel()
            : base()
        {
            var database = ServicesHelper.GetService<ISQLite>();
            if (database != null)
            {
                this.foodDetailDataStoreService = new FoodDetailDataStoreService(database);
            }
        }

        /// <inheritdoc/>
        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            if (parameter is not null)
            {
                this.FoodPreview = parameter as FoodPreviewModel;
                this.FoodPreview.IsRead = true;

                if (this.IsNetworkConnected)
                {
                    this.FoodDetail = await this.dataService.GetFoodDetail(this.FoodPreview?.Id ?? string.Empty);
                }

                if (this.FoodDetail == null)
                {
                    this.FoodDetail = (await this.foodDetailDataStoreService.GetItemsAsync()).FirstOrDefault();
                }
                else
                {
                    await this.foodDetailDataStoreService.AddOrUpdateItemAsync(this.FoodDetail);
                }
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        private void OnBookmarkClicked()
        {
            if (this.FoodPreview == null)
            {
                return;
            }

            this.FoodPreview.BookmarkClickedCommand.Execute(null);
        }
    }
}
