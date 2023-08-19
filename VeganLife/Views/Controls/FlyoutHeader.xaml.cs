// <copyright file="FlyoutHeader.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Controls
{
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Views.Popups;

    public partial class FlyoutHeader : ContentView
    {
        private bool isProcessing;

        public FlyoutHeader()
        {
            this.InitializeComponent();
            DisplayUserInfoPreview();
        }

        private async void DisplayUserInfoPreview()
        {
            var userData = await ServicesHelper.GetService<UserInfoDataStoreServie>().GetFirstOrDefaultItem();
            if (userData != null)
            {
                if (!string.IsNullOrEmpty(userData.Name))
                {
                    lbUserName.Text = userData.Name;
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (isProcessing)
            {
                return;
            }

            isProcessing = true;
            await ServicesHelper.GetService<IPopupNaviService>().PushAsync<ProfilePopup>();
            isProcessing = false;
        }

        private async void AvatarView_Tapped(object sender, TappedEventArgs e)
        {
            if (isProcessing)
            {
                return;
            }

            isProcessing = true;
            Shell.Current.FlyoutIsPresented = false;
            await ServicesHelper.GetService<INavigationService>().NavigateToPage<ProfilePage>();
            isProcessing = false;
        }
    }
}