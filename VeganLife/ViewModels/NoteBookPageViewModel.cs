// <copyright file="NoteBookPageViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.ViewModels
{
    public partial class NoteBookPageViewModel : BaseViewModel, IScrollToTop
    {
        [ObservableProperty]
        private int selectedViewModelIndex = 0;

        [ObservableProperty]
        private MacrosViewModel macrosViewModel;

        [ObservableProperty]
        private VitaminAndMineralViewModel vitaminAndMineralVM;

        [ObservableProperty]
        private AthleticNutritionTabVM athleticNutritionTabVM;

        [ObservableProperty]
        private PharmacoLogicalTabVM pharmacoLogicalTabVM;

        public NoteBookPageViewModel()
            : base()
        {
            MacrosViewModel = ServicesHelper.GetService<MacrosViewModel>();
        }

        [RelayCommand]
        private async Task OpenMenuAsync()
        {
            //if (Shell.Current.CurrentPage is NoteBookPage page)
            //{
            //    await page.AnimateShellMenu();
            //}

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
                    VitaminAndMineralVM ??= ServicesHelper.GetService<VitaminAndMineralViewModel>();
                    _ = VitaminAndMineralVM.ViewAppearingVM();
                    break;
                case 2:
                    AthleticNutritionTabVM ??= ServicesHelper.GetService<AthleticNutritionTabVM>();
                    _ = AthleticNutritionTabVM.ViewAppearingVM();
                    break;
                case 3:
                    PharmacoLogicalTabVM ??= ServicesHelper.GetService<PharmacoLogicalTabVM>();
                    _ = PharmacoLogicalTabVM.ViewAppearingVM();
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
