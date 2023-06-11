using Android.Hardware.Lights;
using ChatGptNet.Exceptions;
using ChatGptNet;
using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Resources.Translations;

namespace VeganLife.Views.Popups;

public partial class BmiResultPopup : Popup
{
    BMIResultModel _bmiResult;

    public BmiResultPopup(BMIResultModel data)
    {
        InitializeComponent();
        _bmiResult = data;
        Init();
        SetupData();
    }

    void Init()
    {
        rootGrid.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.8;
    }

    void SetupData()
    {
        lbBmi.Text = _bmiResult.BMIResult.ToString();
        //string sex = _bmiResult.Sex;
        //string openAIMess = string.Empty;
        try
        {
            //string query = string.Format(AppResources.queryBMI_bmiPopup, sex, _bmiResult.Age, _bmiResult.BMIResult);
            //_ = MainThread.InvokeOnMainThreadAsync(async() =>
            //{
            //    var respone = await ServicesHelper.GetService<IChatGptClient>().AskAsync(System.Guid.NewGuid(), message: query);
            //    if (respone.IsSuccessful)
            //    {
            //        openAIMess = respone.GetMessage();
            //        lbMess.Text = openAIMess;
            //        await Console.Out.WriteLineAsync("==> " + openAIMess);
            //    }
            //}).ConfigureAwait(false);

        }
        catch (ChatGptException chatEX)
        {
            Console.Out.WriteLineAsync("==> " + chatEX.Message);
        }
        catch (Exception)
        {
        }
    }
}