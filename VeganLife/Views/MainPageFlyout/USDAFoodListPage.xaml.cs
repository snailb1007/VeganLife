namespace VeganLife.Views.MainPageFlyout;

public partial class USDAFoodListPage : ContentPage
{
    public USDAFoodListPage(USDAFoodListPageVM uSDAFoodListPageVM)
    {
        InitializeComponent();
        this.BindingContext = uSDAFoodListPageVM;
    }
}