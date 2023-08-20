// <copyright file="NavBarRootPageControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.Views.Controls
{
    public partial class NavBarRootPageControl : ContentView
    {
        public static BindableProperty IsVisibleGreetingContentProperty = BindableProperty.Create(
                propertyName: "IsVisibleGreetingContent",
                declaringType: typeof(NavBarRootPageControl),
                defaultValue: false,
                returnType: typeof(bool));

        public bool IsVisibleGreetingContent
        {
            get => (bool)this.GetValue(IsVisibleGreetingContentProperty);
            set => this.SetValue(IsVisibleGreetingContentProperty, value);
        }

        public NavBarRootPageControl()
        {
            this.InitializeComponent();
            Task.Run(async () =>
            {
                var userName = await ServicesHelper.GetService<IUserDataService>().GetUserNameAsync() ?? "...";
                lbHi.Text = $"Hi {userName}";
            }).Wait();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
            => AppShell.ShowFlyout();

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Width"))
            {
                var img = sender as Image;
                if (img?.Width > 0)
                {
                    Task.Run(async () =>
                    {
                        byte count = 0;
                        while (count < 3)
                        {
                            await img.RelRotateTo(20, 250, Easing.BounceOut);
                            await img.RelRotateTo(-40, 500, Easing.BounceOut);
                            await img.RelRotateTo(20, 250, Easing.BounceOut);
                            count++;
                        }
                    });
                }
            }
        }
    }
}