// <copyright file="FoodDetailPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.FoodTab
{
    using VeganLife.ViewModels.ContentViewModels;
    using VeganLife.Views.Base;

    public partial class FoodDetailPage : BasePage<FoodDetailViewModel>
    {
        // private double marginTopContent;
        private double imgHeight;
        //private double frameTitleHeight;

        //public double MarginTopContent
        //{
        //    get => this.marginTopContent;
        //    set => this.SetProperty(ref this.marginTopContent, value);
        //}

        private double widthOfTabView;
        public double WidthOfTabView
        {
            get => this.widthOfTabView;
            set => SetProperty(ref this.widthOfTabView, value);
        }
        public FoodDetailPage(FoodDetailViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
            WidthOfTabView = App.MainSize / 2;
            //this.SizeChanged += FoodDetailPage_SizeChanged;
        }

        //private void FoodDetailPage_SizeChanged(object sender, EventArgs e)
        //{
        //    this.gridOnTop.HeightRequest = this.cardsViewFoodImage.Height;
        //}

        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("Height"))
            //{
            //    var img = sender as Image;
            //    if (img?.Height > 0)
            //    {
            //        this.imgHeight = img.Height;
            //    }
            //}
        }

        //private void Frame_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{

        //    if (e.PropertyName.Equals("Height"))
        //    {
        //        var frame = sender as Grid;
        //        if (frame?.Height > 0)
        //        {
        //            this.frameTitleHeight = frame.Height;
        //            if (this.imgHeight > 0)
        //            {
        //                this.MarginTopContent = this.imgHeight - (this.frameTitleHeight / 2f);
        //            }
        //        }
        //    }
        //}

        private void VerticalStackLayout_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("Height"))
            //{
            //    var stack = sender as VerticalStackLayout;
            //    (stack as IView).InvalidateMeasure();
            //}
        }

        private void GoNutriFact_SwipeGesture_Swiped(object sender, SwipedEventArgs e)
        {
            //tabView.SelectedTab = tabItemNutritionFacts;
        }

        private void tabItemNutritionFacts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName.Equals("IsSelected") && tabItemNutritionFacts.IsSelected)
            //{
            //    (this.BindingContext as FoodDetailViewModel).GetNutriFactsCommand.Execute(null);
            //}
        }
    }
}