using VeganLife.ViewModels.ToolsFlyoutViewModel;
using VeganLife.Views.Base;

namespace VeganLife.Views.ToolFlyout;

public partial class BMRCalculatorPage
{
    public BMRCalculatorPage(BmrCalculatorViewModel vm)
        : base(vm)
    {
        InitializeComponent();
    }
}