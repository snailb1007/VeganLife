// <copyright file="FlyoutHeader.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Controls
{
    using VeganLife.Data.LocalData;
    using VeganLife.Helpers;
    using VeganLife.Services.UserServices;
    using VeganLife.Views.Popups;

    public partial class FlyoutHeader : ContentView
    {
        private bool isProcessing;
        private readonly IUserDataService userDataService;

        public FlyoutHeader()
        {
            this.InitializeComponent();
            userDataService = ServicesHelper.GetService<IUserDataService>();
            DisplayUserInfoPreview();
        }

        private async void DisplayUserInfoPreview()
        {
            var userData = (this.userDataService as UserDataService)?.UserInfo;
            if (string.IsNullOrEmpty(userData?.Name))
                userData = await ServicesHelper.GetService<UserInfoDataStoreServie>().GetItemAsync();
            if (userData != null)
            {
                if (!string.IsNullOrEmpty(userData.Name))
                {
                    lbUserName.Text = userData.Name;
                }

                avatarViewToolkit.ImageSource = userData.IsMale ? "profile_boy" : "profile_girl_strong";
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