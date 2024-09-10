// <copyright file="ChatGPTDisClaimerPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;

namespace VeganLife.Views.ChatFlyout;

public partial class ChatGPTDisClaimerPage : ContentPage
{
    private readonly INavigationService _navigationService;

    public ChatGPTDisClaimerPage(INavigationService navigationService)
    {
        InitializeComponent();
        this._navigationService = navigationService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var getDataTask = Helpers.ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.chat_disclaimer.txt")
            .ContinueWith(t => lbContent.Text = t.Result);
        getDataTask.SafeFireAndForget();
    }

    private void OnCloseButton_Clicked(object sender, EventArgs e)
    {
        _navigationService.PopAsync().SafeFireAndForget();
    }
}