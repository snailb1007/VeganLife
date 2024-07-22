using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels
{
    public partial class HeightAndWeightAboutPageVM : BaseViewModel
    {
        private InitAboutYouDataRecord _data;

        [ObservableProperty]
        private string height;

        [ObservableProperty]
        private string weight;

        [ObservableProperty]
        private bool isBusy;

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is InitAboutYouDataRecord data)
            {
                _data = data;
            }

            return base.OnNavigatingTo(parameter);
        }

        [RelayCommand]
        private async Task OnSaveClicked()
        {
            if (string.IsNullOrEmpty(Height) || string.IsNullOrEmpty(Weight))
            {
                return;
            }

            if (!float.TryParse(Height, out var height) || !float.TryParse(Weight, out var weight))
            {
                return;
            }

            if (height < 0 || weight < 0)
            {
                return;
            }

            IsBusy = true;
            var user = new UserInfo
            {
                Name = _data.Name,
                DateOfBirth = _data.Birthday,
                IsMale = _data.IsMale,
                Height = (short)height,
                Weight = (float)weight,
            };

            _ = ServicesHelper.GetService<IUserDataService>().SaveData(user);
            _ = UserSettingsHelper.SetAsync(UserSettingKey.IsShowedRegister, true.ToString());

            await (App.Current as App)?.RefreshAppShell()!;
            IsBusy = false;
        }
    }
}
