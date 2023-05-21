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

    private void Button_Clicked(object sender, EventArgs e)
    {
        IsExpanded = !IsExpanded;
    }
}