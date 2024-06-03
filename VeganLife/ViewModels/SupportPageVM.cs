using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.ViewModels
{
    public partial class SupportPageVM : BaseViewModel
    {
        [ObservableProperty]
        private string letter;

        public override async Task ViewAppearingVM()
        {
            Letter ??= await ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.letter_thankYou.txt");
            await base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task OpenCommunityAsync()
        {
            if (Uri.IsWellFormedUriString(ConstantHelper.Community.FacebookLink, UriKind.RelativeOrAbsolute))
            {
                await ServicesHelper.OpenViaBrowserAsync(ConstantHelper.Community.FacebookLink);
            }
        }
    }
}
