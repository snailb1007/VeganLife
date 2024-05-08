// <copyright file="ReportPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.ViewModels
{
    public partial class ReportPageViewModel : BaseViewModel, IScrollToTop
    {
        [ObservableProperty]
        private int _selectedViewModelIndex = 0;
        [ObservableProperty]
        private string numberBadgeCalories;
        [ObservableProperty]
        private string numberBadgeCart;

        public CaloriesViewModel CaloriesViewModel { get; }

        [ObservableProperty]
        private MacrosViewModel _macrosViewModel;

        public NutrientsViewModel NutrientsViewModel { get; }

        public ReportPageViewModel()
            : base()
        {
            NumberBadgeCalories = "99";
            NumberBadgeCart = string.Empty;
            CaloriesViewModel = ServicesHelper.GetService<CaloriesViewModel>();
            MacrosViewModel = ServicesHelper.GetService<MacrosViewModel>();
            NutrientsViewModel = ServicesHelper.GetService<NutrientsViewModel>();
        }

        [RelayCommand]
        private void OpenMenu()
        {
            AppShell.ShowFlyOut();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            await MacrosViewModel.ViewAppearingVM();
            return base.ViewAppearingVM();
        }

        partial void OnSelectedViewModelIndexChanged(int value)
        {
            switch (value)
            {
                case 1:
                    _ = CaloriesViewModel.ViewAppearingVM();
                    break;
                case 0:
                    _ = MacrosViewModel.ViewAppearingVM();
                    break;
                case 2:
                    _ = NutrientsViewModel.ViewAppearingVM();
                    break;
            }
        }

        public void ScrollToTop()
        {
            if (SelectedViewModelIndex == 0)
            {
                MacrosViewModel.ScrollToTop();
            }
        }
    }
}
