namespace VeganLife.Views.ContentViews;

public partial class ElementFoodDetailCW : ContentView
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

    public static BindableProperty IsExpandedProperty = BindableProperty.Create(
            propertyName: "IsExpanded",
            declaringType: typeof(ElementFoodDetailCW),
            defaultValue: true,
            returnType: typeof(bool));
    public bool IsIsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public ElementFoodDetailCW()
	{
		InitializeComponent();
        BindingContext = this;
	}
}