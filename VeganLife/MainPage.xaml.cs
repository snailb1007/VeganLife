namespace VeganLife;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

    protected override bool OnBackButtonPressed()
    {
        Console.WriteLine("==>OnBackButtonPressed");
        return base.OnBackButtonPressed();
    }
}

