
using VeganLife.Helpers;
using VeganLife.Views.AboutYou;

namespace VeganLife.ViewModels
{
    public partial class NameAboutUPageVM : BaseViewModel
    {
        public override Task ViewAppearingVM()
        {
            ServicesHelper.GetService<IDeviceService>().SetNavigationBarColor("#144d5a");
            //_ = UserSettingsHelper.SetAsync(UserSettingKey.IsShowedRegister, true.ToString());
            return base.ViewAppearingVM();
        }

        [RelayCommand]
        public async Task OnNextClicked()
        {
            using (await this.loadingService.Show())
            {
                await App.Current?.MainPage?.Navigation?.PushAsync(ServicesHelper.GetService<BirthdayAboutPage>())!;
            }
        }
    }
}