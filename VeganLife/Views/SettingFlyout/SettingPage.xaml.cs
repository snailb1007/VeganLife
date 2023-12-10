// <copyright file="SettingPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.SettingFlyout
{
    using VeganLife.Views.Base;

    public partial class SettingPage : BasePage<SettingViewModel>
    {
        public SettingPage(SettingViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }
    }
}