namespace VeganLife.Views;

public partial class BMICalculatorPage : ContentPage
{
	public BMICalculatorPage(BMICalculatorViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void Grid_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
		if (e.PropertyName.Equals("IsVisible"))
		{
            Console.WriteLine("==> " + lbHelpSex.Height);
		}
    }
}