using VeganLife.Views.Base;

namespace VeganLife.Views.AboutYou;

public partial class NameAboutUPage : BasePage<NameAboutUPageVM>
{
    public NameAboutUPage(NameAboutUPageVM vm)
        : base(vm)
    {
        InitializeComponent();
    }
}