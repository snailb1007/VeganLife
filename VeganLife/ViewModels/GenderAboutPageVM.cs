using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class GenderAboutPageVM : BaseViewModel
    {
        [ObservableProperty]
        private bool _isMale;

        [RelayCommand]
        private void SelectGender(string param)
        {
            IsMale = param == "1";
        }

        [RelayCommand]
        private async Task OnNextClicked()
        {
            await this.navigationService.NavigateToPage<HeightAndWeightAboutPage>();
        }
    }
}
