using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.SettingTab;

public partial class LicensePage : ContentPage
{
    public LicensePage(LicenseViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}