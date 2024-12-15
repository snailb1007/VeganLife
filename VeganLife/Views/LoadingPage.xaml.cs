using AsyncAwaitBestPractices;
using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
using VeganLife.Views.AboutYou;
using VeganLife.Views.Popups;

namespace VeganLife.Views;

public partial class LoadingPage
{
    public LoadingPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        FFImageLoading.Helpers.ServiceHelper.GetService<IDataService>().GetHealthDiagnosisFirebaseDataModel()
            .ContinueWith(t =>
            {
                StaticHelper.HealthDiagnosisFirebaseDataModel.BMIModel = t.Result;
            })
            .SafeFireAndForget();
        FFImageLoading.Helpers.ServiceHelper.GetService<IUserDataService>().InitAsync().SafeFireAndForget();
    }

    private void SelfContentLoaded(object sender, EventArgs e)
    {
        var getLocalFlagTask = UserSettingsHelper.GetBoolKey(UserSettingKey.HasPriorInstances);
        getLocalFlagTask.ContinueWith(async t =>
        {
            if (!t.Result)
            {
                UserSettingsHelper.SetAsync(UserSettingKey.HasPriorInstances, true.ToString()).SafeFireAndForget();
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var isCollectAccepted = await this.DisplayAlert(
                    string.Empty,
                    message: AppResources.Alert_CollectOperationLogsPermission_Message,
                    accept: AppResources.ok_common,
                    cancel: AppResources.cancel_common);
                    FFImageLoading.Helpers.ServiceHelper.GetService<SentryService>().IsEnabled = isCollectAccepted;
                    await UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, isCollectAccepted.ToString());
                });
            }

            if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.IsAcceptedTermsAndConditions))
            {
                var aboutView = FFImageLoading.Helpers.ServiceHelper.GetService<AboutAppPopup>();
                MainThread.BeginInvokeOnMainThread(async () => await this.ShowPopupAsync(aboutView));
                await aboutView.WaitingAcceptedTaskSource.Task;
            }

            if (Application.Current is App app)
            {
                //if (true)
                if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.IsShowedRegister))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        app.Windows[0].Page = new NavigationPage(FFImageLoading.Helpers.ServiceHelper.GetService<NameAboutUPage>());
                    });
                }
                else
                {
                    await app.RefreshAppShell();
                }
            }
        }).SafeFireAndForget();
    }
}