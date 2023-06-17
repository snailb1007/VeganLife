using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class SettingPage : BasePage<SettingViewModel>
{
    public SettingPage(SettingViewModel vm) : base(vm)
    {
        InitializeComponent();
        _viewModel = vm;
    }

    readonly SettingViewModel _viewModel;

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        var x = sender as Microsoft.Maui.Controls.Switch;
        _viewModel.SwitchThemeCommand.Execute(x);
    }
}