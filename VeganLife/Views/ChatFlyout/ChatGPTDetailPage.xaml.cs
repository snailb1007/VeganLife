namespace VeganLife.Views.ChatFlyout;

public partial class ChatGPTDetailPage : ContentPage
{
    private INavigationService _navigationService;
    public ChatGPTDetailPage(INavigationService navigationService)
    {
        InitializeComponent();
        this._navigationService = navigationService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.Dispatch(async () =>
        {
            lbContent.Text = await Helpers.ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.chat_detail.txt");
        });
    }

    private async void OnCloseButton_Clicked(object sender, EventArgs e)
    {
        await _navigationService.PopAsync();
    }
}