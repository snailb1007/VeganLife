using CommunityToolkit.Maui.Views;

namespace VeganLife.Views.Popups;

public partial class BmiMoreInfoToolBarPopup : Popup
{
	public BmiMoreInfoToolBarPopup()
	{
		InitializeComponent();
		mainGrid.WidthRequest = App.MainSize * 0.8;
	}

    private void Close_TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		this.Close();
    }
}