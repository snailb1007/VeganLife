using VeganLife.Helpers;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.ViewModels
{
    public partial class ReportPageViewModel : BaseViewModel
    {
        private int _selectedViewModelIndex = 0;
        public int SelectedViewModelIndex
        {
            get => _selectedViewModelIndex;
            set => SetAndRaise(ref _selectedViewModelIndex, value);
        }

        public CaloriesViewModel CaloriesViewModel { get; }
        public MacrosViewModel MacrosViewModel { get; }
        public NutrientsViewModel NutrientsViewModel { get; }
        public ReportPageViewModel()
            : base()
        {
            CaloriesViewModel = new CaloriesViewModel();
            MacrosViewModel = ServicesHelper.GetService<MacrosViewModel>();
            NutrientsViewModel = new NutrientsViewModel();
        }

        public override async Task<Task> OnNavigatingTo(object parameter)
        {
            var t1 = CaloriesViewModel.OnNavigatingTo(parameter);
            var t2 = MacrosViewModel.OnNavigatingTo(parameter);
            var t3 = NutrientsViewModel.OnNavigatingTo(parameter);
            await Task.WhenAll(t1, t2, t3);
            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        void MenuClicked()
        {
            MacrosViewModel.ClickedThisTabCommand.Execute(null);
        }
    }
}
