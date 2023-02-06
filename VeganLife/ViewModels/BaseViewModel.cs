using CommunityToolkit.Mvvm.ComponentModel;
using VeganLife.Services;

namespace VeganLife.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INavigationService navigationService;
        public BaseViewModel()
        {
        }
    }
}
