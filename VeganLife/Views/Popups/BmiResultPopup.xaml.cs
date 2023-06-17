using ChatGptNet;
using ChatGptNet.Exceptions;
using Mopups.Pages;
using Mopups.Services;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;

namespace VeganLife.Views.Popups;

public partial class BmiResultPopup : PopupPage
{
    BMIResultModel _bmiResult;

    public BmiResultPopup(BMIResultModel data)
    {
        InitializeComponent();
        _bmiResult = data;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await SetupDataAsync();
    }

    async Task SetupDataAsync()
    {
        lbBmi.Text = _bmiResult.BMIResult.ToString();
        lbMess.Text = "The AddChatGpt method has also an overload that accepts an IServiceProvider as argument. It can be used, for example, if we're in a Web API and we need to support scenarios in which every user has a different API Key that can be retrieved accessing a database via Dependency Injection:";
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