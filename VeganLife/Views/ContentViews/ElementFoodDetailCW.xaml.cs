using VeganLife.Models.FoodModel;

namespace VeganLife.Views.ContentViews;

public partial class ElementFoodDetailCW : ContentView, INotifyPropertyChanged
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

    bool _isExpanded = true;
    //public bool IsExpanded
    //{
    //    get => _isExpanded;
    //    set
    //    {
    //        _isExpanded = value;
    //        OnPropertyChanged(nameof(IsExpanded));
    //    }
    //}

    public ElementFoodDetailCW()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        _isExpanded = !_isExpanded;
        expander.IsExpanded = _isExpanded;
        var source = _isExpanded ? Application.Current.Resources["IconAngleDown"]
            : Application.Current.Resources["IconAngleUp"];
        btnExpanedStatus.Text = source as string;
    }
}