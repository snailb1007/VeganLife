using VeganLife.Helpers.Extensions;
using VeganLife.Views.Base;

namespace VeganLife.Views.MainPageFlyout;

public partial class MealLogsPage : BasePage<MealLogsPageVM>, IBaseRootPage
{
    public MealLogsPage(MealLogsPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }

    public bool IsAnimated { get; set; }

    public void OnClosedShellFlyout()
    {
        this.AnimateCloseShellMenu(mainGridContent);
    }

    public void OnOpenedShellFlyout()
    {
        this.AnimateShellMenu(mainGridContent);
    }
}