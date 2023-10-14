using UraniumUI.Pages;
using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.PortionTab;

public partial class UsdaFoodFactDetailPage : UraniumContentPage
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
}