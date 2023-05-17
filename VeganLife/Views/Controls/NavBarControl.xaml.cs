using VeganLife.Helpers;

namespace VeganLife.Views.Controls;

public partial class NavBarControl : ContentView
{
    public static BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: "Title",
            declaringType: typeof(NavBarControl),
            defaultValue: null,
            returnType: typeof(string));
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static BindableProperty IsShowBookmarkToolProperty = BindableProperty.Create(
            propertyName: "IsShowBookmarkTool",
            declaringType: typeof(NavBarControl),
            defaultValue: false,
            returnType: typeof(bool));
    public bool IsShowBookmarkTool
    {
        get => (bool)GetValue(IsShowBookmarkToolProperty);
        set => SetValue(IsShowBookmarkToolProperty, value);
    }

    public static BindableProperty IsBookmarkedProperty = BindableProperty.Create(
            propertyName: "IsBookmarked",
            declaringType: typeof(NavBarControl),
            defaultValue: false,
            returnType: typeof(bool));
    public bool IsBookmarked
    {
        get => (bool)GetValue(IsBookmarkedProperty);
        set => SetValue(IsBookmarkedProperty, value);
    }

    public NavBarControl()
	{
		InitializeComponent();
	}

    async void Back_Clicked(object sender, EventArgs e)
    {
        await ServicesHelper.GetService<INavigationService>().PopAsync();
    }
}