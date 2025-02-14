using AsyncAwaitBestPractices;

namespace VeganLife.Views.ChatFlyout;

public partial class ChatGPTDetailPage
{
    private readonly INavigationService _navigationService;

    public ChatGPTDetailPage(INavigationService navigationService)
    {
        InitializeComponent();
        this._navigationService = navigationService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var getDataTask = Helpers.ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.chat_detail.txt")
            .ContinueWith(t => lbContent.Text = t.Result);
        getDataTask.SafeFireAndForget();
    }

    private void OnCloseButton_Clicked(object sender, EventArgs e)
    {
        _navigationService.PopAsync().SafeFireAndForget();
    }
}