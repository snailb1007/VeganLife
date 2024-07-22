using VeganLife.Views.Base;

namespace VeganLife.Views.AboutYou;

public partial class GenderAboutPage : BasePage<GenderAboutPageVM>
{
    public GenderAboutPage(GenderAboutPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}