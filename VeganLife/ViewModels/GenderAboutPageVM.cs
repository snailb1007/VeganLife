using VeganLife.Helpers;
using VeganLife.Services.UserServices;
using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class GenderAboutPageVM : BaseViewModel
    {
        private InitAboutYouDataRecord _data;

        [ObservableProperty]
        private bool _isMale;

        public override Task OnNavigatingTo(object parameter)
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
#if DEV || DEBUG
            var user = new UserInfo
            {
                Name = _data.Name,
                DateOfBirth = _data.Birthday,
                IsMale = this.IsMale,
                Height = 170,
                Weight = 57,
            };

            await FFImageLoading.Helpers.ServiceHelper.GetService<IUserDataService>().SaveData(user);
            _ = UserSettingsHelper.SetAsync(UserSettingKey.IsShowedRegister, true.ToString());

            await (Application.Current as App)?.RefreshAppShell()!;
#else
            await this.navigationService.NavigateToPage<HeightAndWeightAboutPage>(
                new InitAboutYouDataRecord(_data.Name, _data.Birthday, IsMale));
#endif
        }
    }
}
