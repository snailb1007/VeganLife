// <copyright file="DetailVitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout.VitaminTab
{
    public partial class DetailVitaminAndMineralPage : BasePage<DetailVitaminAndMineralViewModel>
    {
        private double xOffset, yOffset;

        public DetailVitaminAndMineralPage(DetailVitaminAndMineralViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    ideaFrame.TranslationX = xOffset + e.TotalX;
                    ideaFrame.TranslationY = yOffset + e.TotalY;
                    break;

                case GestureStatus.Completed:
                    xOffset = ideaFrame.TranslationX;
                    yOffset = ideaFrame.TranslationY;
                    break;
            }
        }
    }
}