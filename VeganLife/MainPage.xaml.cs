namespace VeganLife;

public partial class MainPage : TabbedPage
{
	public MainPage(MainViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}

