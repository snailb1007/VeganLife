namespace VeganLife;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        Console.WriteLine("==>OnBackButtonPressed");
        return base.OnBackButtonPressed();
    }

    bool _processing;
    private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
    {
        if (_processing)
            return;
        _processing = true;
        var menu = sender as CarouselView;
        foreach(var i in menu?.VisibleViews)
        {
            var img = i.FindByName<Image>("imgMenu");
            if (img == null)
                return;
            Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(img);
            Task.Run(async () => await img.RelRotateTo(360, 5000, Easing.CubicInOut));
        }

        _processing = false;
    }
}

