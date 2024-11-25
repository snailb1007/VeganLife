// <copyright file="AboutAppPopup.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using VeganLife.Helpers;

namespace VeganLife.Views.Popups;

public partial class AboutAppPopup
{
    public TaskCompletionSource<bool> WaitingAcceptedTaskSource { get; set; }

    public AboutAppPopup()
    {
        this.InitializeComponent();
        rootGrid.WidthRequest = App.MainWidthSize * 0.8;
        lbVersion.Text = AppInfo.Current.VersionString;
        WaitingAcceptedTaskSource = new TaskCompletionSource<bool>();
    }

    private void TermsAndConditions_Tapped(object sender, TappedEventArgs e)
    {
        Browser.Default.OpenAsync("https://veganhealthies.wordpress.com/2023/07/23/dieu-khoan-dieu-kien/").SafeFireAndForget();
    }

    private void PrivacyPolicy_Tapped(object sender, TappedEventArgs e)
    {
        Browser.Default.OpenAsync("https://veganhealthies.wordpress.com/2023/07/23/chinh-sach-quyen-rieng-tu/").SafeFireAndForget();
    }

    private void Close_Clicked(object sender, EventArgs e)
    {
        this.Close();
        UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedTermsAndConditions, true.ToString())
            .ContinueWith(t => WaitingAcceptedTaskSource.SetResult(true))
            .SafeFireAndForget();
    }
}