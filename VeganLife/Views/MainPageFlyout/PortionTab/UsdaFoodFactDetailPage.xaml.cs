using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.PortionTab;

public partial class UsdaFoodFactDetailPage : ContentPage
{
	public UsdaFoodFactDetailPage(UsdaFoodFactDetailVM vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = (BindingContext as UsdaFoodFactDetailVM).ViewAppearingVM();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as UsdaFoodFactDetailVM).ViewDisappearingVM();
    }
}