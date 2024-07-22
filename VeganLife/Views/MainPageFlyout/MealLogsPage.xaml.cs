using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class MealLogsPage : BasePage<MealLogsPageVM>
{
    public MealLogsPage(MealLogsPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}