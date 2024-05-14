using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;

namespace VeganLife.ViewModels
{
    public partial class SupportPageVM : BaseViewModel
    {
        [RelayCommand]
        private async Task OpenCommunityAsync()
        {
            if (Uri.IsWellFormedUriString(ConstantHelper.Community.FacebookLink, UriKind.RelativeOrAbsolute))
            {
                await ServicesHelper.OpenViaBrowserAsync(ConstantHelper.Community.FacebookLink);
            }
        }

        [RelayCommand]
        private async Task ShareAppAsync()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = ConstantHelper.AppStoreLink,
                Title = AppResources.app_name,
                Text = "Invite your friends",
            });
        }
    }
}
