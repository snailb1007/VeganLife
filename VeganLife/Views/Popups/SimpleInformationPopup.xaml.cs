using Mopups.Pages;

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
}