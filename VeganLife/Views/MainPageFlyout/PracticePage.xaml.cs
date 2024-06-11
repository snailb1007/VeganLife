namespace VeganLife.Views.MainPageFlyout;

public partial class PracticePage : ContentPage
{
    public PracticePage(PracticePageVM vm)
    {
        InitializeComponent();
        this.BindingContext = vm;
    }
}