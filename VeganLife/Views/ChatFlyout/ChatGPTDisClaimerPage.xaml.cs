// <copyright file="ChatGPTDisClaimerPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;

namespace VeganLife.Views.ChatFlyout;

public partial class ChatGPTDisClaimerPage : ContentPage
{
    private INavigationService _navigationService;

    public ChatGPTDisClaimerPage(INavigationService navigationService)
    {
        InitializeComponent();
        this._navigationService = navigationService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.Dispatch(async () =>
        {
            lbContent.Text = await ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.chat_disclaimer.txt");
        });
    }

    private async void OnCloseButton_Clicked(object sender, EventArgs e)
    {
        await _navigationService.PopAsync();
    }
}