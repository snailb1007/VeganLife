using VeganLife.Helpers;
using VeganLife.Models.FoodModel;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.ViewModels
{
    public partial class ReportPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        private int _selectedViewModelIndex = 0;

        public CaloriesViewModel CaloriesViewModel { get; }
        public MacrosViewModel MacrosViewModel { get; }
        public NutrientsViewModel NutrientsViewModel { get; }
        public ReportPageViewModel()
            : base()
        {
            CaloriesViewModel = ServicesHelper.GetService<CaloriesViewModel>();
            MacrosViewModel = ServicesHelper.GetService<MacrosViewModel>();
            NutrientsViewModel = ServicesHelper.GetService<NutrientsViewModel>();
        }

        [RelayCommand]
        private void OpenMenu()
        {
            AppShell.ShowFlyout();
        }

        partial void OnSelectedViewModelIndexChanged(int value)
        {
            switch (value)
            {
                case 0:
                    _ = CaloriesViewModel.ViewAppearingVM();
                    break;
                case 1:
                    _ = MacrosViewModel.ViewAppearingVM();
                    break;
                case 2:
                    _ = NutrientsViewModel.ViewAppearingVM();
                    break;
            }
        }
    }
}
