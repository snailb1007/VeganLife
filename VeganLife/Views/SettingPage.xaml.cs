using VeganLife.ViewModels;

namespace VeganLife.Views;

public partial class SettingPage : ContentPage
{
	public SettingPage(SettingViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	private SettingViewModel _viewModel => BindingContext as SettingViewModel;

    private void Switch_Toggled(object sender, ToggledEventArgs e)
	{
		var x = sender as Switch;
		_viewModel.SwitchThemeCommand.Execute(x);
    }
}