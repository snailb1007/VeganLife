// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Resources.Translations;
using VeganLife.Views.Base;
using VeganLife.Views.Popups;

namespace VeganLife
{
    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class MainPage : BasePage<MainViewModel>, IBaseRootPage
    {
        private readonly MainViewModel viewModel;

        public bool IsAnimated { get; set; }

        public MainPage(MainViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            this.viewModel = vm;
        }

        private void gridTransparent_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            this.searchBar.Unfocus();
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            viewModel.LoadDataCommand.Execute(null);
            if (sender is RefreshView refreshView)
            {
                refreshView.IsRefreshing = false;
            }
        }

        private async void mainPageRoot_LoadedAsync(object sender, EventArgs e)
        {
            if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.HasPriorInstances))
            {
                await UserSettingsHelper.SetAsync(UserSettingKey.HasPriorInstances, true.ToString()).ConfigureAwait(false);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var isCollectAccepted = await this.DisplayAlert(
                    string.Empty,
                    message: AppResources.Alert_CollectOperationLogsPermission_Message,
                    accept: AppResources.ok_common,
                    cancel: AppResources.cancel_common);
                    ServicesHelper.GetService<SentryService>().IsEnabled = isCollectAccepted;
                    await UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, isCollectAccepted.ToString()).ConfigureAwait(false);
                });
            }

            if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.IsAcceptedTermsAndConditions))
            {
                Dispatcher.Dispatch(async () => await this.ShowPopupAsync(ServicesHelper.GetService<AboutAppPopup>()));
            }
        }

        public void OnOpenedShellFlyout()
        {
            this.AnimateShellMenu(mainGridContent);
        }

        public void OnClosedShellFlyout()
        {
            this.AnimateCloseShellMenu(mainGridContent);
        }
    }
}