using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class USDAFoodListPage : BasePage<USDAFoodListPageVM>
{
    public USDAFoodListPage(USDAFoodListPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}