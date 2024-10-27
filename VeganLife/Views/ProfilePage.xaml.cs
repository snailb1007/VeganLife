// <copyright file="ProfilePage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class ProfilePage : BasePage<ProfileViewModel>
{
    public ProfilePage(ProfileViewModel vm)
        : base(vm)
    {
        this.InitializeComponent();
    }
}