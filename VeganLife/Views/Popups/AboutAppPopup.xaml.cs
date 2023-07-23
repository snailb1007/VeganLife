using CommunityToolkit.Maui.Views;

namespace VeganLife.Views.Popups;

public partial class AboutAppPopup : Popup
{
	public AboutAppPopup()
	{
		this.InitializeComponent();
		rootGrid.WidthRequest = App.MainSize * 0.8;
        lbVersion.Text = AppInfo.Current.VersionString;
	}
}