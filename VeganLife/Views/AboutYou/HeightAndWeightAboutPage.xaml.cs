using VeganLife.Views.Base;

namespace VeganLife.Views.AboutYou;

public partial class HeightAndWeightAboutPage : BasePage<HeightAndWeightAboutPageVM>
{
    public HeightAndWeightAboutPage(HeightAndWeightAboutPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}