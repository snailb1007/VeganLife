// <copyright file="DetailVitaminAndMineralPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.MainPageFlyout.VitaminTab
{
    public partial class DetailVitaminAndMineralPage
    {
        private double _xOffset;
        private double _yOffset;

        public DetailVitaminAndMineralViewModel ViewModel { get; private set; }

        public DetailVitaminAndMineralPage(DetailVitaminAndMineralViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            ViewModel = vm;
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    // Save initial translation offsets
                    _xOffset = ideaFrame.TranslationX;
                    _yOffset = ideaFrame.TranslationY;
                    break;

                case GestureStatus.Running:
                    ideaFrame.TranslationX = _xOffset + e.TotalX;
                    ideaFrame.TranslationY = _yOffset + e.TotalY;
                    break;

                case GestureStatus.Completed:
                    _xOffset = ideaFrame.TranslationX;
                    _yOffset = ideaFrame.TranslationY;
                    (ideaFrame as IView).InvalidateMeasure();
                    break;
            }
        }
    }
}