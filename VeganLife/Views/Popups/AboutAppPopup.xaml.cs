using VeganLife.ViewModels.PopupViewModels;

namespace VeganLife.Views.Popups;

public partial class AboutAppPopup : ContentPage
{
	public AboutAppPopup(AboutAppPopupViewModel vm)
	{
		this.InitializeComponent();
		this.BindingContext = vm;
	}
}