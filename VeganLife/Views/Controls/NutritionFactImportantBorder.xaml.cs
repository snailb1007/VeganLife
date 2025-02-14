namespace VeganLife.Views.Controls;

public partial class NutritionFactImportantBorder
{
    public static BindableProperty TitleProperty { get; set; } = BindableProperty.Create(
                propertyName: "Title",
                declaringType: typeof(NavBarControl),
                defaultValue: null,
                returnType: typeof(string));

    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    public NutritionFactImportantBorder()
    {
        InitializeComponent();
    }
}