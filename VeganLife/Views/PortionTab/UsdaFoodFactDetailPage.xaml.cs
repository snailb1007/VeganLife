using VeganLife.ViewModels.ContentViewModels;
using VeganLife.Views.Base;

namespace VeganLife.Views.PortionTab;

public partial class UsdaFoodFactDetailPage : BasePage<UsdaFoodFactDetailVM>
{
	public UsdaFoodFactDetailPage(UsdaFoodFactDetailVM vm)
		: base(vm)
	{
		InitializeComponent();
	}
}