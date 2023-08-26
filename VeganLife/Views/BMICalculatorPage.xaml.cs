// <copyright file="BMICalculatorPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views
{
    public partial class BMICalculatorPage : ContentPage
    {
        public BMICalculatorPage(BMICalculatorViewModel vm)
        {
            this.InitializeComponent();
            this.BindingContext = vm;
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            (slider.Handler.PlatformView as Android.Widget.SeekBar).ContentDescription = "this is slider";
        }
    }
}