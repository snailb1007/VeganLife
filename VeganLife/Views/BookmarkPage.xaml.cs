namespace VeganLife.Views;

public partial class BookmarkPage : ContentPage
{
	public BookmarkPage(BookmarkViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}