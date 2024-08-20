// <copyright file="FlyoutHeader.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CommunityToolkit.Mvvm.Messaging;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.messages;
using VeganLife.Resources.Translations;
using VeganLife.Services.UserServices;
using VeganLife.Views.Popups;

// Ignore Spelling: Flyout
namespace VeganLife.Views.Controls
{
    public partial class FlyoutHeader : ContentView, IRecipient<ProfileChangedMessage>
    {
        private readonly IUserDataService _userDataService;
        private bool _isProcessing;

        public bool IsMale { get; set; }

        private readonly IEnumerable<string> grettingList;

        public FlyoutHeader()
        {
            this.InitializeComponent();
            grettingList = new List<string>() { AppResources.prompt_greeting, AppResources.prompt_greeting_v1 };
            _userDataService = ServicesHelper.GetService<IUserDataService>();
            _ = DisplayUserInfoPreview();
            WeakReferenceMessenger.Default.Register(this);
        }

        private async Task DisplayUserInfoPreview()
        {
            var userData = this._userDataService.GetUserInfo();
            if (string.IsNullOrEmpty(userData?.Name))
            {
                userData = await ServicesHelper.GetService<UserInfoDataStoreServie>().GetFirstOrDefaultItem();
            }

            if (userData != null)
            {
                if (!string.IsNullOrEmpty(userData.Name))
                {
                    lbUserName.Text = userData.Name;
                    var randomGreetingIndex = new Random().Next(grettingList.Count());
                    lbGreeting.Text = string.Format(grettingList.ElementAt(randomGreetingIndex), userData.Name);
                }

                imgAvatar.Source = userData.Image;
            }
        }

        private async void AvatarView_Tapped(object sender, TappedEventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            _isProcessing = true;
            Shell.Current.FlyoutIsPresented = false;
            await ServicesHelper.GetService<INavigationService>().NavigateToPage<ProfilePage>();
            _isProcessing = false;
        }

        public void Receive(ProfileChangedMessage message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await this.DisplayUserInfoPreview();
            });
        }

        private async void OnEditProfileClicked(object sender, TappedEventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            _isProcessing = true;
            await ServicesHelper.GetService<IPopupNaviService>().PushAsync<ProfilePopup>();
            _isProcessing = false;
        }
    }
}