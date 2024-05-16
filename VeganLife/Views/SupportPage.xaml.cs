namespace VeganLife.Views;

public partial class SupportPage : ContentPage
{
    public SupportPage(SupportPageVM vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
}