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
        private MacrosViewModel _macrosViewModel;

        [ObservableProperty]
        private VitaminAndMineralViewModel _vitaminAndMineralVM;

        public ReportPageViewModel()
            : base()
        {
            MacrosViewModel = ServicesHelper.GetService<MacrosViewModel>();
            VitaminAndMineralVM = ServicesHelper.GetService<VitaminAndMineralViewModel>();
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
                    _ = VitaminAndMineralVM.ViewAppearingVM();
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
