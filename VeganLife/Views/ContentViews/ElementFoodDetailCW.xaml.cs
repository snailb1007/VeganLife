using VeganLife.Views.ContentViews.Base;

namespace VeganLife.Views.ContentViews;

public partial class ElementFoodDetailCW : BaseContentView
{
    public static BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: "Title",
            declaringType: typeof(ElementFoodDetailCW),
            defaultValue: null,
            returnType: typeof(string));
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static BindableProperty ContentExpandProperty = BindableProperty.Create(
            propertyName: "ContentExpand",
            declaringType: typeof(ElementFoodDetailCW),
            defaultValue: null,
            returnType: typeof(string));
    public string ContentExpand
    {
        get => (string)GetValue(ContentExpandProperty);
        set => SetValue(ContentExpandProperty, value);
    }

    bool _isExpanded = true;
    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetProperty(ref _isExpanded, value);
    }

    public ElementFoodDetailCW()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName.Equals("Title"))
        {
            string img = string.Empty;
            switch (Title)
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

            imgTitle.Source = img;
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        IsExpanded = !IsExpanded;
    }
}