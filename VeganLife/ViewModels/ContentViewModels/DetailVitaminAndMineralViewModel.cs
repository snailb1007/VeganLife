// <copy
// right file="DetailVitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    /// <summary>
    /// vm of DetailVitaminAndMineralPage.
    /// </summary>
    public partial class DetailVitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private VitaminModel vitamin;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailVitaminAndMineralViewModel"/> class.
        /// </summary>
        public DetailVitaminAndMineralViewModel()
            : base()
        {
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is VitaminModel vitamin)
            {
                this.Vitamin = vitamin;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        private async Task ItemSelectedAsync(byte param)
        {
            if (ItemSelectedCommand.IsRunning || param < 0)
            {
                return;
            }

            IsLoading = true;
            var scrollView = Shell.Current.CurrentPage.FindByName<ScrollView>("DetailAthleticNutritionScrollView");
            if (scrollView != null)
            {
                var stackLayout = scrollView.Content as StackLayout;
                var mainStackLayout = stackLayout?.Children.OfType<VerticalStackLayout>().FirstOrDefault();
                var target = mainStackLayout?.Children
                    .OfType<Label>()
                    .FirstOrDefault(label => label.AutomationId == param.ToString());
                if (target != null)
                {
                    await scrollView.ScrollToAsync(target, ScrollToPosition.Start, animated: true);
                }
            }

            IsLoading = false;
        }
    }
}
