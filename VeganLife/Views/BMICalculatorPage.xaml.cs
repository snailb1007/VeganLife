namespace VeganLife.Views;

public partial class BMICalculatorPage : ContentPage
{
    public BMICalculatorPage(BMICalculatorViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}