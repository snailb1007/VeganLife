using VeganLife.Views.MainPageFlyout;

namespace VeganLife.ViewModels
{
    public partial class PracticePageVM : BaseViewModel
    {
        [RelayCommand]
        private async Task OnPoseDetectionClickedAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                return;
            }

            await this.navigationService.NavigateToPage<VisionPage>();
        }
    }
}
