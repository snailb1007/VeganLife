using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class GenderAboutPageVM : BaseViewModel
    {
        private InitAboutYouDataRecord _data;

        [ObservableProperty]
        private bool _isMale;

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is InitAboutYouDataRecord data)
            {
                _data = data;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        private void SelectGender(string param)
        {
            IsMale = param == "1";
        }

        [RelayCommand]
        private async Task OnNextClicked()
        {
            await this.navigationService.NavigateToPage<HeightAndWeightAboutPage>(
                new InitAboutYouDataRecord(_data.Name, _data.Birthday, IsMale));
        }
    }
}
