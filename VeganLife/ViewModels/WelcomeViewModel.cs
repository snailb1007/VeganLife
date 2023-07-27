using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
using VeganLife.Views.Popups;

namespace VeganLife.ViewModels
{
    public class WelcomeImage
    {
        public WelcomeImage(string image)
        {
            this.Image = image;
        }
        public string Image { get; set; }
    }

    public partial class WelcomeViewModel : BaseViewModel
    {
        [ObservableProperty]
        IList<WelcomeImage> _listImage;

        public WelcomeViewModel()
            : base()
        {
            Init();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            if (UserSettingsHelper.IsFirstTime)
            {
                UserSettingsHelper.Set(UserSettingKey.IsFirstTime, false.ToString());
                await Task.Delay(1);
                var isCollectAccepted = await this.navigationService.DisplayAlert(
                    string.Empty,
                    message: AppResources.Alert_CollectOperationLogsPermission_Message,
                    ok: AppResources.ok_common,
                    cancel: AppResources.cancel_common);
                UserSettingsHelper.Set(UserSettingKey.IsAcceptedCollectLogs, isCollectAccepted.ToString());
                App.Current.MainPage.ShowPopup(ServicesHelper.GetService<AboutAppPopup>());
            }

            App.SetupCollectLogPermission();

            return base.ViewAppearingVM();
        }


        void Init()
        {
            ListImage = new List<WelcomeImage>
            {
                new WelcomeImage("slide_image1.jpg"),
                new WelcomeImage("slide_image2.jpg"),
                new WelcomeImage("slide_image3.jpg")
            };
        }

        [RelayCommand]
        void Skip()
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}
