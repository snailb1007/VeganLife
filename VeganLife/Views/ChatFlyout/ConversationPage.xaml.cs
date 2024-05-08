// <copyright file="ConversationPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.ChatFlyout;

public partial class ConversationPage : ContentPage
{
    public ConversationPage(ConversationViewModel viewModel)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await (this.BindingContext as ConversationViewModel)?.ViewAppearingVM()!;
    }
}