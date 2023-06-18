// <copyright file="SettingPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views
{
    using VeganLife.Views.Base;

    public partial class SettingPage : BasePage<SettingViewModel>
    {
        public SettingPage(SettingViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var x = sender as Microsoft.Maui.Controls.Switch;
            (this.BindingContext as SettingViewModel).SwitchThemeCommand.Execute(x);
        }
    }
}