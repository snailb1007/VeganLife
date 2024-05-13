// <copyright file="ElementFoodDetailCW.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Views.ContentViews.Base;

namespace VeganLife.Views.ContentViews
{
    public partial class ElementFoodDetailCW : BaseContentView
    {
        public static BindableProperty TitleProperty = BindableProperty.Create(
                propertyName: "Title",
                declaringType: typeof(ElementFoodDetailCW),
                defaultValue: null,
                returnType: typeof(string));

        public string Title
        {
            get => (string)this.GetValue(TitleProperty);
            set => this.SetValue(TitleProperty, value);
        }

        public static BindableProperty ContentExpandProperty = BindableProperty.Create(
                propertyName: "ContentExpand",
                declaringType: typeof(ElementFoodDetailCW),
                defaultValue: null,
                returnType: typeof(string));

        public string ContentExpand
        {
            get => (string)this.GetValue(ContentExpandProperty);
            set => this.SetValue(ContentExpandProperty, value);
        }

        private bool isExpanded = true;

        public bool IsExpanded
        {
            get => this.isExpanded;
            set => this.SetProperty(ref this.isExpanded, value);
        }

        public ElementFoodDetailCW()
        {
            this.InitializeComponent();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName.Equals("Title"))
            {
                string img = string.Empty;
                switch (this.Title)
                {
                    case "Nguyên liệu":
                        img = "ingredients_food_detail";
                        break;
                    case "Cách làm":
                        img = "cooking_food_detail";
                        break;
                    case "Nước sốt":
                        img = "sauce_food_detail";
                        break;
                    case "Trang trí":
                        img = "decorate_food_detail";
                        break;
                }

                this.imgTitle.Source = img;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            this.IsExpanded = !this.IsExpanded;
        }
    }
}