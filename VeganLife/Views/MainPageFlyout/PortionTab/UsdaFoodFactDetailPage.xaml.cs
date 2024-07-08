using VeganLife.Helpers;
using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.PortionTab;

public partial class UsdaFoodFactDetailPage : ContentPage
{
    private readonly UsdaFoodFactDetailVM _vm;

    public bool IsDataGridExpanded { get; set; }

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

    private void ContentPage_SizeChanged(object sender, EventArgs e)
    {
        headerImgGrid.MaximumHeightRequest = (this.Height / 2) - navBarContentView.Height;
    }
}