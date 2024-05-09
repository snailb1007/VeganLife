// <copyright file="FoodsByCategoryViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Models.FoodModel;
using VeganLife.Views.MainPageFlyout.FoodTab;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class FoodsByCategoryViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string titlePage;
        [ObservableProperty]
        private FoodPreviewModel currentSelectedItem;

        [ObservableProperty]
        private IEnumerable<FoodPreviewModel> foods;

        public FoodsByCategoryViewModel()
            : base()
        {
        }

        /// <inheritdoc/>
        public override Task OnNavigatingTo(object? parameter)
        {
            var data = parameter as Dictionary<string, IEnumerable<FoodPreviewModel>>;
            if (data != null)
            {
                this.TitlePage = data.FirstOrDefault().Key;
                this.Foods = data.FirstOrDefault().Value;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        private async Task GoFoodDetail()
        {
            await this.navigationService.NavigateToPage<FoodDetailPage>(this.CurrentSelectedItem);
            this.CurrentSelectedItem = null;
        }
    }
}
