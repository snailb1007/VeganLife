// <copyright file="NavBarControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Controls
{
    using VeganLife.Helpers;

    public partial class NavBarControl : ContentView
    {
        public static BindableProperty TitleProperty = BindableProperty.Create(
                propertyName: "Title",
                declaringType: typeof(NavBarControl),
                defaultValue: null,
                returnType: typeof(string));

        public string Title
        {
            get => (string)this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }

        public NavBarControl()
        {
            this.InitializeComponent();
        }

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await ServicesHelper.GetService<INavigationService>().PopAsync();
        }

        private void hamburger_Clicked(object sender, EventArgs e)
            => AppShell.ShowFlyout();
    }
}