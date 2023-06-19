// <copyright file="BmiResultPopup.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Popups
{
    using Mopups.Pages;
    using Mopups.Services;
    using VeganLife.ViewModels.PopupViewModels;

    public partial class BmiResultPopup : PopupPage
    {
        public BmiResultPopup(BmiResultPopupViewmodel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }

        async Task SetupDataAsync()
        {
            await Task.Delay(1);
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
}