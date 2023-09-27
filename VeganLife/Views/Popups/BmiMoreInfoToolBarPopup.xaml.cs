using CommunityToolkit.Maui.Views;
using VeganLife.ViewModels.PopupViewModels;

namespace VeganLife.Views.Popups;

public partial class BmiMoreInfoToolBarPopup : Popup
{
	public BmiMoreInfoToolBarPopup(BmiMoreInfoToolBarPopupVM vm)
	{
		InitializeComponent();
		mainGrid.WidthRequest = App.MainSize * 0.8;
		this.BindingContext = vm;
    }

    private void Close_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		this.Close();
    }
}