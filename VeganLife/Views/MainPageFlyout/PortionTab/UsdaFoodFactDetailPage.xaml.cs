using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.PortionTab;

public partial class UsdaFoodFactDetailPage : ContentPage
{
    private readonly UsdaFoodFactDetailVM _vm;

    public UsdaFoodFactDetailPage(UsdaFoodFactDetailVM vm)
    {
        this.BindingContext = _vm = vm;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.Dispatch(async () =>
        {
            await _vm.ViewAppearingVM();
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Dispatcher.Dispatch(async () =>
        {
            await _vm.ViewDisappearingVM();
        });
    }
}