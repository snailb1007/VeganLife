namespace VeganLife.Views;

public partial class NewsFeedPage : ContentPage
{
    readonly NewsFeedViewModel _viewModel;
	public NewsFeedPage(NewsFeedViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
        _viewModel = vm;
	}

    private void DiscoverMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender == null)
            return;
        _viewModel.SelectDiscoveryMenuCommand.Execute(e.CurrentSelection);
        collectionFeeds.ScrollTo(0, 0);
    }
}