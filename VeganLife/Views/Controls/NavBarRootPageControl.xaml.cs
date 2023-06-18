namespace VeganLife.Views.Controls;

public partial class NavBarRootPageControl : ContentView
{
    public static BindableProperty IsVisibleGreetingContentProperty = BindableProperty.Create(
            propertyName: "IsVisibleGreetingContent",
            declaringType: typeof(NavBarRootPageControl),
            defaultValue: false,
            returnType: typeof(bool));
    public bool IsVisibleGreetingContent
    {
        get => (bool)GetValue(IsVisibleGreetingContentProperty);
        set => SetValue(IsVisibleGreetingContentProperty, value);
    }

    public NavBarRootPageControl()
    {
        InitializeComponent();
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
        => AppShell.ShowFlyout();

    private void Image_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName.Equals("Width"))
        {
            var img = sender as Image;
            if (img?.Width > 0)
            {
                Task.Run(async () =>
                {
                    byte count = 0;
                    while (count < 3)
                    {
                        await img.RelRotateTo(20, 250, Easing.BounceOut);
                        await img.RelRotateTo(-40, 500, Easing.BounceOut);
                        await img.RelRotateTo(20, 250, Easing.BounceOut);
                        count++;
                    }
                });
            }
        }
    }
}