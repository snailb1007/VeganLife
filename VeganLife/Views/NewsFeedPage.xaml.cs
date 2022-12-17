using VeganLife.ViewModels;

namespace VeganLife.Views;

public partial class NewsFeedPage : ContentPage
{
	public NewsFeedPage(NewsFeedViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}