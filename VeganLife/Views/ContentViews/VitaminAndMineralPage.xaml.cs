using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.ContentViews;

public partial class VitaminAndMineralPage : ContentPage
{
    VitaminAndMineralViewModel _viewModel;

    public VitaminAndMineralPage(VitaminAndMineralViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _viewModel = vm;
	}
}