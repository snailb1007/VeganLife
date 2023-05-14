using CommunityToolkit.Maui.Behaviors;

namespace VeganLife;

public partial class MainPage : ContentPage
{
    readonly MainViewModel _viewModel;
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _viewModel = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!_viewModel.IsLoadDataOnAppearingDone)
            _viewModel.LoadDataCommand.Execute(null);
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
            Task.Run(async () => await img.RelRotateTo(360, 5000, Easing.BounceOut));
        }

        _processing = false;
    }
}

