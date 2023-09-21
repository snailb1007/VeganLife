// <copyright file="BMICalculatorPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.Base;

namespace VeganLife.Views.ToolFlyout
{
    public partial class MainTool : BasePage<MainToolViewModel>
    {
        public MainTool(MainToolViewModel vm) : base(vm)
        {
            this.InitializeComponent();
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            (slider.Handler.PlatformView as Android.Widget.SeekBar).ContentDescription = "this is slider";
        }
    }
}