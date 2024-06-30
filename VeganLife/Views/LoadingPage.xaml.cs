using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;
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
            await this.ShowPopupAsync(aboutView);
            await aboutView.WaitingAcceptedTaskSource.Task;
        }

        if (Application.Current?.MainPage is not null)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}