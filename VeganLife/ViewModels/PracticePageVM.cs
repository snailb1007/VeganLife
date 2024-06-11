using VeganLife.Views.MainPageFlyout;

namespace VeganLife.ViewModels
{
    public partial class PracticePageVM : BaseViewModel
    {
        [RelayCommand]
        private async Task OnPoseDetectionClickedAsync()
        {
            await this.navigationService.NavigateToPage<VisionPage>();
        }
    }
}
