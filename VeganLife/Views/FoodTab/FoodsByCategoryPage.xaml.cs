using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.FoodTab;

public partial class FoodsByCategoryPage : ContentPage
{
	public FoodsByCategoryPage(FoodsByCategoryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}