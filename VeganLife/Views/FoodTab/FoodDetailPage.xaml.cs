using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.FoodTab;

public partial class FoodDetailPage : ContentPage
{
	public FoodDetailPage(FoodDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}