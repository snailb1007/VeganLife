namespace VeganLife.Views;

public partial class NewsFeedPage : ContentPage
{
	public NewsFeedPage()
	{ }
	public NewsFeedPage(NewsFeedViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}