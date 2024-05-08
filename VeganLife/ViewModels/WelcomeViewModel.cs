namespace VeganLife.ViewModels
{
    public partial class WelcomeViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IList<TutorialModel> _listImage;

        [ObservableProperty]
        private byte _flowViewSelectedIndex;

        public WelcomeViewModel()
            : base()
        {
            Init();
        }

        //public override async Task<Task> ViewAppearingVM()
        //{
            //var tutorialData = await Helpers.ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.tutorial.txt");
            //var items = StringProcessHelper.ParseSections(tutorialData);
            //if (items != null && items.Any())
            //{
            //    for (int i = 0; i < 3; i++)
            //    {
            //        var target = ListImage.ElementAt(i);
            //        var data = items.ElementAt(i);
            //        target.Title = data.Title;
            //        target.Description = data.Description;
            //    }
            //}

            //if (UserSettingsHelper.IsFirstTime)
            //{
            //    UserSettingsHelper.SetAsync(UserSettingKey.IsFirstTime, false.ToString());
            //    await Task.Delay(1);
            //    var isCollectAccepted = await this.navigationService.DisplayAlert(
            //        string.Empty,
            //        message: AppResources.Alert_CollectOperationLogsPermission_Message,
            //        ok: AppResources.ok_common,
            //        cancel: AppResources.cancel_common);
            //    UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, isCollectAccepted.ToString());
            //    App.Current.MainPage.ShowPopup(ServicesHelper.GetService<AboutAppPopup>());
            //}

            //App.SetupCollectLogPermission();

            //return base.ViewAppearingVM();
        //}

        private void Init()
        {
            //ListImage = new List<TutorialModel>
            //{
            //    new TutorialModel() { ImageLink = "https://firebasestorage.googleapis.com/v0/b/vegan-life-d1c9b.appspot.com/o/step1.webp?alt=media&token=f44e2020-0501-4de1-8fc5-5f1d3f37e88e" },
            //    new TutorialModel() { ImageLink = "https://firebasestorage.googleapis.com/v0/b/vegan-life-d1c9b.appspot.com/o/step2.webp?alt=media&token=adf95f44-172c-47c1-b1fb-a6b893ee0263" },
            //    new TutorialModel() { ImageLink = "https://firebasestorage.googleapis.com/v0/b/vegan-life-d1c9b.appspot.com/o/step3.webp?alt=media&token=9fc5072b-9e2d-4a34-b7bb-2c93ca6ad811" }
            //};
        }

        [RelayCommand]
        private void Skip()
        {
            // if (Application.Current is not null)
            // {
            //    Application.Current.MainPage = new AppShell();
            // }
        }

        public class TutorialModel : BaseDataModel
        {
            public required string ImageLink { get; set; }
        }

        [RelayCommand]
        private void ManualSwipeCard(string param)
        {
            // if (param.Equals("0") && FlowViewSelectedIndex > 0)
            // {
            //    FlowViewSelectedIndex--;
            // }else if (param.Equals("1") && FlowViewSelectedIndex < ListImage.Count -1)
            // {
            //    FlowViewSelectedIndex++;
            // }
        }
    }
}
