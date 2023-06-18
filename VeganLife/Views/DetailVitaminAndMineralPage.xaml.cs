using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views;

public partial class DetailVitaminAndMineralPage : ContentPage
{
    public DetailVitaminAndMineralPage(DetailVitaminAndMineralViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}