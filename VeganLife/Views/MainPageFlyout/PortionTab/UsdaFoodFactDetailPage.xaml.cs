using VeganLife.Helpers.Extensions;
using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.MainPageFlyout.PortionTab;

public partial class UsdaFoodFactDetailPage
{
    private readonly UsdaFoodFactDetailVM _vm;

    public bool IsDataGridExpanded { get; set; }

    public UsdaFoodFactDetailPage(UsdaFoodFactDetailVM vm)
    {
        InitializeComponent();
        this.BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.Dispatch(async void () =>
        {
            try
            {
                await _vm.ViewAppearingVM();
            }
            catch (Exception e)
            {
                e.LogError();
            }
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Dispatcher.Dispatch(async void () =>
        {
            try
            {
                await _vm.ViewDisappearingVM();
            }
            catch (Exception e)
            {
                e.LogError();
            }
        });
    }

    private void ContentPage_SizeChanged(object sender, EventArgs e)
    {
        headerImgGrid.MaximumHeightRequest = (this.Height / 2) - navBarContentView.Height;
    }
}