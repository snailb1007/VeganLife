// <copyright file="ConversationPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;

namespace VeganLife.Views.ChatFlyout;

public partial class ConversationPage
{
    public ConversationPage(ConversationViewModel viewModel)
    {
        this.BindingContext = viewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (this.BindingContext as ConversationViewModel)?.ViewAppearingVM().SafeFireAndForget();
    }
}