
using VeganLife.Helpers;

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
    }
}