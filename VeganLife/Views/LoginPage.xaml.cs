using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class LoginPage : BasePage<LoginViewModel>
{
    public LoginPage(LoginViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}