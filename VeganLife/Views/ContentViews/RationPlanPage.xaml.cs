using VeganLife.ViewModels.ContentViewModels;

namespace VeganLife.Views.ContentViews;

public partial class RationPlanPage : ContentPage
{
	public RationPlanPage(RationPlanViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}