// <copyright file="NavBarControl.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
using Mopups.Services;
using System.Net.NetworkInformation;
using VeganLife.Helpers;
using VeganLife.Views.Popups;

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

        public static BindableProperty AffiliationsProperty = BindableProperty.Create(
                propertyName: nameof(Affiliations),
                declaringType: typeof(NavBarControl),
                defaultValue: null,
                returnType: typeof(List<AffiliationModel>));

        public List<AffiliationModel> Affiliations
        {
            get => (List<AffiliationModel>)this.GetValue(AffiliationsProperty);
            set => this.SetValue(AffiliationsProperty, value);
        }

        public NavBarControl()
        {
            this.InitializeComponent();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(Affiliations))
            {
                affiliationsShoFrame.IsVisible = Affiliations != null && Affiliations.Any();
            }
        }

        private bool _isProcessing;

        private void Back_Clicked(object sender, EventArgs e)
        {
            if (_isProcessing)
            {
                return;
            }

            _isProcessing = true;
            ServicesHelper.GetService<INavigationService>().PopAsync().SafeFireAndForget();
            _isProcessing = false;
        }

        private void hamburger_Clicked(object sender, EventArgs e)
            => AppShell.ShowFlyOut();

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            MopupService.Instance.PushAsync(new AffiliationPopup(Affiliations)).SafeFireAndForget();
        }
    }
}