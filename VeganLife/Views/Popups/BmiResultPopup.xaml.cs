using Mopups.Pages;
using Mopups.Services;

namespace VeganLife.Views.Popups;

public partial class BmiResultPopup : PopupPage
{
    private readonly BMIResultModel bmiResult;

    public BmiResultPopup(BMIResultModel data)
    {
        InitializeComponent();
        this.bmiResult = data;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await SetupDataAsync();
    }

    async Task SetupDataAsync()
    {
        await Task.Delay(1);
        this.lbBmi.Text = this.bmiResult.BMIResult.ToString();
        this.lbMess.Text = "The AddChatGpt method has also an overload that accepts an IServiceProvider as argument. It can be used, for example, if we're in a Web API and we need to support scenarios in which every user has a different API Key that can be retrieved accessing a database via Dependency Injection:";
        //string sex = _bmiResult.Sex;
        //string openAIMess = string.Empty;
        //try
        //{
        //    string query = string.Format(AppResources.queryBMI_bmiPopup, sex, _bmiResult.Age, _bmiResult.BMIResult);
        //    await ServicesHelper.GetService<IChatGptClient>()
        //        .AskAsync(System.Guid.NewGuid(), message: query)
        //        .ContinueWith((t) =>
        //    {
        //        if (t.Result.IsSuccessful)
        //        {
        //            openAIMess = t.Result.GetMessage();
        //            lbMess.Text = openAIMess;
        //        }
        //    }).ConfigureAwait(true);
        //}
        //catch (ChatGptException chatEX)
        //{
        //    await Console.Out.WriteLineAsync("==> " + chatEX.Message);
        //}
        //catch (Exception)
        //{
        //}
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await MopupService.Instance.PopAsync();
    }
}