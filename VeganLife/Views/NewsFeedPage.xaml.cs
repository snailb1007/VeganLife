using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class NewsFeedPage : BasePage<NewsFeedViewModel>
{
    readonly NewsFeedViewModel _viewModel;
    public NewsFeedPage(NewsFeedViewModel vm) : base(vm)
    {
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