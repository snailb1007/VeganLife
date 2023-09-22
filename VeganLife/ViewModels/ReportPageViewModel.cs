using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.ViewModels
{
    public class ReportPageViewModel : BaseViewModel
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
            MacrosViewModel = new MacrosViewModel();
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
    }
}
