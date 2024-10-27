using AsyncAwaitBestPractices;
using VeganLife.Helpers.AppSetting;
using VeganLife.Helpers.Extensions;
using VeganLife.Resources.Translations;

namespace VeganLife.Views.ContentViews;

public partial class ShellFlyoutFooterCustom : ContentView
{
    public ShellFlyoutFooterCustom()
    {
        InitializeComponent();
    }

    private void OnButtonShareTapped(object sender, TappedEventArgs e)
    {
        var requestTask = Share.RequestAsync(new ShareTextRequest
        {
            Uri = ConstantHelper.AppStoreLink,
            Title = AppResources.app_name,
            Text = "Invite your friends",
        });
        requestTask.SafeFireAndForget(onException: ex => ex.LogError());
    }
}