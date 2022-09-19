using VeganLife.ViewModels;

namespace VeganLife.Views;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	private void Switch_Toggled(object sender, ToggledEventArgs e)
	{
		var x = sender as Switch;
		if (x != null && x.IsToggled)
			App.Current.UserAppTheme = AppTheme.Dark;
		else
            App.Current.UserAppTheme = AppTheme.Light;
    }
}