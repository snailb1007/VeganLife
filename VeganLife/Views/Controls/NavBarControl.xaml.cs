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

    public NavBarControl()
    {
        InitializeComponent();
    }

    async void Back_Clicked(object sender, EventArgs e)
    {
        await ServicesHelper.GetService<INavigationService>().PopAsync();
    }

    private void hamburger_Clicked(object sender, EventArgs e)
        => AppShell.ShowFlyout();
}