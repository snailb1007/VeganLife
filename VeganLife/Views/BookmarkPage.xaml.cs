// <copyright file="BookmarkPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views
{
    using VeganLife.Views.Base;

    public partial class BookmarkPage : BasePage<BookmarkViewModel>
    {
        public BookmarkPage(BookmarkViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }
    }
}