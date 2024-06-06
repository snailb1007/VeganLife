using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class SupportPage : BasePage<SupportPageVM>
{
    public SupportPage(SupportPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}