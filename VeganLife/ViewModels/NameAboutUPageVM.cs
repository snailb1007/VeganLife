
using VeganLife.Helpers;
using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class NameAboutUPageVM : BaseViewModel
    {
        [ObservableProperty]
        private string name;

        public bool IsFilledName => !string.IsNullOrEmpty(Name) && !string.IsNullOrWhiteSpace(Name);

        public override Task ViewAppearingVM()
        {
            ServicesHelper.GetService<IDeviceService>().SetNavigationBarColor("#144d5a");
            return base.ViewAppearingVM();
        }

        [RelayCommand]
        public async Task OnNextClicked()
        {
            if (NextClickedCommand.IsRunning)
            {
                return;
            }

            await this.navigationService.NavigateToPage<BirthdayAboutPage>(paramater: Name);
        }
    }
}