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
        private string _titlePage;
        [ObservableProperty]
        private FoodPreviewModel _currentSelectedItem;

        [ObservableProperty]
        private IEnumerable<FoodPreviewModel> _foods;

        /// <inheritdoc/>
        public override Task OnNavigatingTo(object parameter)
        {
            if (parameter is not Dictionary<string, IEnumerable<FoodPreviewModel>> data)
            {
                return base.OnNavigatingTo(parameter);
            }

            this.TitlePage = data.FirstOrDefault().Key;
            this.Foods = data.FirstOrDefault().Value;

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        private async Task GoFoodDetail()
        {
            await this.navigationService.NavigateToPage<FoodDetailPage>(paramater: this.CurrentSelectedItem);
            this.CurrentSelectedItem = null!;
        }
    }
}
