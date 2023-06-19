// <copyright file="FoodsByCategoryViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    using VeganLife.Models.FoodModel;
    using VeganLife.Views.FoodTab;

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
        public override Task OnNavigatingTo(object parameter)
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
            await this.navigationService.NavigataToPage<FoodDetailPage>(this.CurrentSelectedItem);
            this.CurrentSelectedItem = null;
        }
    }
}
