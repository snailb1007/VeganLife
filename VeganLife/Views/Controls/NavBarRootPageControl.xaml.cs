// <copyright file="NavBarRootPageControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: Nav
using CommunityToolkit.Mvvm.Messaging;
using VeganLife.Helpers;
using VeganLife.messages;
using VeganLife.Services.UserServices;

namespace VeganLife.Views.Controls
{
    public partial class NavBarRootPageControl : ContentView, IRecipient<ProfileChangedMessage>
    {
        public static BindableProperty IsVisibleGreetingContentProperty = BindableProperty.Create(
                propertyName: "IsVisibleGreetingContent",
                declaringType: typeof(NavBarRootPageControl),
                defaultValue: false,
                returnType: typeof(bool));

        public static BindableProperty HamburgerSourceProperty = BindableProperty.Create(
               propertyName: "HamburgerSource",
               declaringType: typeof(NavBarRootPageControl),
               defaultValue: "hamburger",
               returnType: typeof(string));

        public bool IsVisibleGreetingContent
        {
            get => (bool)this.GetValue(IsVisibleGreetingContentProperty);
            set => this.SetValue(IsVisibleGreetingContentProperty, value);
        }

        public string HamburgerSource
        {
            get => (string)this.GetValue(HamburgerSourceProperty);
            set => this.SetValue(HamburgerSourceProperty, value);
        }

        public NavBarRootPageControl()
        {
            this.InitializeComponent();
            _ = this.LoadUserDataAsync();
            WeakReferenceMessenger.Default.Register(this);
        }

        private async Task LoadUserDataAsync()
        {
            var userName = await ServicesHelper.GetService<IUserDataService>().GetUserNameAsync() ?? "...";
            lbHi.Text = $"Hi {userName}";
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
            => AppShell.ShowFlyOut();

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName?.Equals("Width") ?? false)
            {
                var img = sender as Image;
                if (img?.Width > 0)
                {
                    Task.Run(async () =>
                    {
                        byte count = 0;
                        while (count < 3 && img.IsVisible)
                        {
                            await img.RelRotateTo(20, 250, Easing.BounceOut);
                            await img.RelRotateTo(-40, 500, Easing.BounceOut);
                            await img.RelRotateTo(20, 250, Easing.BounceOut);
                            count++;
                        }

                        MainThread.BeginInvokeOnMainThread(() => img.IsVisible = false);
                    });
                }
            }
        }

        public void Receive(ProfileChangedMessage message)
        {
            MainThread.BeginInvokeOnMainThread(async () => await LoadUserDataAsync());
        }
    }
}