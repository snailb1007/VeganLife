namespace VeganLife.Views.Controls;

public partial class NavBarRootPageControl : ContentView
{
	public NavBarRootPageControl()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
		await Task.Delay(1);
		MainThread.BeginInvokeOnMainThread(() =>
		{
            (App.Current.MainPage as AppShell).FlyoutIsPresented = true;
        });
    }
}