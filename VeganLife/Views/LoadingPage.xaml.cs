using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
using VeganLife.Views.AboutYou;
using VeganLife.Views.Popups;
using static VeganLife.Helpers.AppSetting.StaticHelper;

namespace VeganLife.Views;

public partial class LoadingPage : ContentPage
{
    public LoadingPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = ServicesHelper.GetService<IDataService>().GetHealthDiagnosisFirebaseDataModel()
            .ContinueWith(t =>
            {
                HealthDiagnosisFirebaseDataModel.BMIModel = t.Result;
            });
        _ = ServicesHelper.GetService<IUserDataService>().Refresh();
        _ = ServicesHelper.GetService<IUserDataService>().Init();
    }

    private async void SelfContentLoaded(object sender, EventArgs e)
    {
        if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.HasPriorInstances))
        {
            await UserSettingsHelper.SetAsync(UserSettingKey.HasPriorInstances, true.ToString()).ConfigureAwait(false);
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var isCollectAccepted = await this.DisplayAlert(
                string.Empty,
                message: AppResources.Alert_CollectOperationLogsPermission_Message,
                accept: AppResources.ok_common,
                cancel: AppResources.cancel_common);
                ServicesHelper.GetService<SentryService>().IsEnabled = isCollectAccepted;
                await UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, isCollectAccepted.ToString()).ConfigureAwait(false);
            });
        }

        if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.IsAcceptedTermsAndConditions))
        {
            var aboutView = ServicesHelper.GetService<AboutAppPopup>();
            MainThread.BeginInvokeOnMainThread(async () => await this.ShowPopupAsync(aboutView));
            await aboutView.WaitingAcceptedTaskSource.Task;
        }

        if ((App.Current as App) is App app)
        {
            //if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.IsShowedRegister))
            if (true)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    app.MainPage = new NavigationPage(ServicesHelper.GetService<NameAboutUPage>());
                });
            }
            else
            {
                await app.RefreshAppShell();
            }
        }
    }
}