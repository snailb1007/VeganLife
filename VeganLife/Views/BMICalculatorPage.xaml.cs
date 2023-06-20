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
    }
}