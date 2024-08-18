using Mopups.Pages;
using Mopups.Services;

namespace VeganLife.Views.Popups;

public partial class SimpleInformationPopup : PopupPage
{
    public SimpleInformationPopup(string info)
    {
        InitializeComponent();
        if (string.IsNullOrEmpty(info))
        {
            return;
        }

        lbInfo.Text = info;
    }

    private void OutsidePopup_Tapped(object sender, TappedEventArgs e)
    {
        _ = MopupService.Instance.PopAsync();
    }
}