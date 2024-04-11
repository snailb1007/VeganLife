using CommunityToolkit.Maui.Views;
using VeganLife.Helpers;

namespace VeganLife.Views.Popups;

public partial class AboutAppPopup : Popup
{
	public AboutAppPopup()
	{
		this.InitializeComponent();
		rootGrid.WidthRequest = App.MainSize * 0.8;
        lbVersion.Text = AppInfo.Current.VersionString;
	}

    private void TermsAndConditions_Tapped(object sender, TappedEventArgs e)
    {
        Browser.Default.OpenAsync("https://veganhealthies.wordpress.com/2023/07/23/dieu-khoan-dieu-kien/");
    }

    private void PrivacyPolicy_Tapped(object sender, TappedEventArgs e)
    {
        Browser.Default.OpenAsync("https://veganhealthies.wordpress.com/2023/07/23/chinh-sach-quyen-rieng-tu/");
    }

    private void Close_Clicked(object sender, EventArgs e)
    {
        this.Close();
        UserSettingsHelper.Set(UserSettingKey.IsAcceptedTermsAndConditions, true.ToString());
    }
}