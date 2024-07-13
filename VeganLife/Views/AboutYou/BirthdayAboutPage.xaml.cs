using VeganLife.Views.Base;

namespace VeganLife.Views.AboutYou;

public partial class BirthdayAboutPage : BasePage<BirthdayAboutPageVM>
{
    public BirthdayAboutPage(BirthdayAboutPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}