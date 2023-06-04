namespace VeganLife.Views;

public partial class WebViewPage : ContentPage
{
	public WebViewPage(WebViewViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		AppShell.ShowFlyout();
    }
}