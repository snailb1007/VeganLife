using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
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
        ServicesHelper.GetService<IDeviceService>().SetNavigationBarColor("#144d5a");
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
            var t1 = ServicesHelper.GetService<IDataService>().GetAllAffiliations();
            await Task.WhenAll(Task.Delay(500), t1);
            StaticHelper.Affiliation.Affiliations = t1.Result.ToList();
            MainThread.BeginInvokeOnMainThread(() => app.MainPage = new AppShell());
        }
    }
}