// <copyright file="ProfilePage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace VeganLife.Views;

using VeganLife.Views.Base;
public partial class ProfilePage : BasePage<ProfileViewModel>
{
    public ProfilePage(ProfileViewModel vm)
        : base(vm)
    {
        this.InitializeComponent();
    }
}