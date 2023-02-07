namespace VeganLife.Views;

public partial class WebViewPage : ContentPage
{
	public WebViewPage(WebViewViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}