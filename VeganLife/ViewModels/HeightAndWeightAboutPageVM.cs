using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels
{
    public partial class HeightAndWeightAboutPageVM : BaseViewModel
    {
        private InitAboutYouDataRecord _data;

        [ObservableProperty]
        private string _height;

        [ObservableProperty]
        private string _weight;

        [ObservableProperty]
        private bool _sBusy;

        public override Task OnNavigatingTo(object parameter)
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

            busyManager.Increase();
            var user = new UserInfo
            {
                Name = _data.Name,
                DateOfBirth = _data.Birthday,
                IsMale = _data.IsMale,
                Height = (short)height,
                Weight = (float)weight,
            };

            await ServicesHelper.GetService<IUserDataService>().SaveData(user);
            _ = UserSettingsHelper.SetAsync(UserSettingKey.IsShowedRegister, true.ToString());

            await (Application.Current as App)?.RefreshAppShell()!;
            busyManager.Decrease();
        }
    }
}
