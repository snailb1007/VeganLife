using VeganLife.Services.LocalDataServices;

namespace VeganLife;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}

