// <copyright file="ReportPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.ViewModels.TabsViewModel;
using VeganLife.Views.ContentViews.Tabs;

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

        [ObservableProperty]
        private MacrosViewModel _macrosViewModel;

        public ReportPageViewModel()
            : base()
        {
            MacrosViewModel = ServicesHelper.GetService<MacrosViewModel>();
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
                case 0:
                    _ = MacrosViewModel.ViewAppearingVM();
                    break;
                case 1:
                    //_ = NutrientsViewModel.ViewAppearingVM();
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
