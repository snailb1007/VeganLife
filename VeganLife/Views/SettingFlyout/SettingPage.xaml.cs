// <copyright file="SettingPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.Base;

namespace VeganLife.Views.SettingFlyout
{
    public partial class SettingPage : BasePage<SettingViewModel>
    {
        public SettingPage(SettingViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }
    }
}