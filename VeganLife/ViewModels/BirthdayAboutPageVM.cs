using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class BirthdayAboutPageVM : BaseViewModel
    {
        public DateTime MaximumDateOfBirth => DateTime.Now.AddDays(-30);

        [RelayCommand]
        public async Task OnContinueClicked()
        {
            await this.navigationService.NavigateToPage<GenderAboutPage>();
        }
    }
}