namespace VeganLife.Views.AboutYou;

public partial class NameAboutUPage
{
    public NameAboutUPage(NameAboutUPageVM vm)
        : base(vm)
    {
        vm.NavigationViewModel = this.Navigation;
        InitializeComponent();
    }
}