namespace VeganLife.Views;

public partial class NewsFeedPage : ContentPage
{
    NewsFeedViewModel _viewModel;
	public NewsFeedPage(NewsFeedViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}

    private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender == null)
            return;
        collectionFeeds.ScrollTo(0, 0);
        _viewModel.SelectDiscoveryMenuCommand.Execute(e.CurrentSelection);
    }
}