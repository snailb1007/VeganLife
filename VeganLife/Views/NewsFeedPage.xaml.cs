namespace VeganLife.Views;

public partial class NewsFeedPage : ContentPage
{
	public NewsFeedPage(NewsFeedViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}