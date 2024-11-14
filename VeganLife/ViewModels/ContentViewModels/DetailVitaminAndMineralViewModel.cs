// <copy
// right file="DetailVitaminAndMineralViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Mopups.Services;
using VeganLife.Helpers.AppSetting;
using VeganLife.Views.Popups;

namespace VeganLife.ViewModels.ContentViewModels
{
    /// <summary>
    /// vm of DetailVitaminAndMineralPage.
    /// </summary>
    public partial class DetailVitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        private VitaminModel _vitamin;

        [ObservableProperty]
        private List<AffiliationModel> _affiliations;

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
                Affiliations = (StaticHelper.Affiliation.Affiliations.Where(i => i.NutrientName == vitamin.Id)
                    ?? []).ToList();
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
        }

        [RelayCommand]
        private async Task OnRecommendedClicked()
        {
            await MopupService.Instance.PushAsync(new AffiliationPopup(Affiliations));
        }
    }
}