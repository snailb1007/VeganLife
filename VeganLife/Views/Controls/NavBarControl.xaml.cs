// <copyright file="NavBarControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;

namespace VeganLife.Views.Controls
{
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

        private bool _isProcessing;

        private async void Back_Clicked(object sender, EventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            _isProcessing = true;
            await ServicesHelper.GetService<INavigationService>().PopAsync();
            _isProcessing = false;
        }

        private void hamburger_Clicked(object sender, EventArgs e)
            => AppShell.ShowFlyOut();
    }
}