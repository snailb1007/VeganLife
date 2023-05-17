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

    FoodDetailModel _foodDetail;
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (BindingContext != null )
        {
            _foodDetail = BindingContext as FoodDetailModel;
            if (_foodDetail != null )
            {
                lbContent.Text = _foodDetail.Ingredient;
            }
        }
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