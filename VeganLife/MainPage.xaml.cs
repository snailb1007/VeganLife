// <copyright file="MainPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife
{
    using CommunityToolkit.Maui.Views;
    using VeganLife.Helpers;
    using VeganLife.Resources.Translations;
    using VeganLife.Views.Base;
    using VeganLife.Views.Popups;

    /// <summary>
    /// auto-generated.
    /// </summary>
    public partial class MainPage : BasePage<MainViewModel>
    {
        private readonly MainViewModel viewModel;

        public MainPage(MainViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            this.viewModel = vm;
        }

        // private bool processing;

        //private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        //{
        //    if (this.processing)
        //    {
        //        return;
        //    }

        //    this.processing = true;
        //    var menu = sender as CarouselView;
        //    foreach (var i in menu?.VisibleViews)
        //    {
        //        var img = i.FindByName<Image>("imgMenu");
        //        if (img == null)
        //        {
        //            return;
        //        }

        //        Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(img);
        //        Task.Run(async () => await img.RelRotateTo(360, 5000, Easing.BounceOut));
        //    }

        //    this.processing = false;
        //}

        private void gridTransparent_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            this.searchBar.Unfocus();
        }

        private void RefreshView_Refreshing(object sender, EventArgs e)
        {
            viewModel.LoadDataCommand.Execute(null);
            (sender as RefreshView).IsRefreshing = false;
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
                    await UserSettingsHelper.SetAsync(UserSettingKey.IsAcceptedCollectLogs, isCollectAccepted.ToString()).ConfigureAwait(false);
                });
            }

            if (!await UserSettingsHelper.GetBoolKey(UserSettingKey.IsAcceptedTermsAndConditions))
            {
                Dispatcher.Dispatch(async() => await this.ShowPopupAsync(ServicesHelper.GetService<AboutAppPopup>()));
            }
        }
    }
}