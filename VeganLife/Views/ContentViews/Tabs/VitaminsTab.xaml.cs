using VeganLife.Helpers.AppSetting;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class VitaminsTab : ContentView
{
    public VitaminsTab()
    {
        InitializeComponent();
        SetupAdsBanner();
    }

    private void SetupAdsBanner()
    {
        mtAdFixed.AdsId = ConstantHelper.GoogleAdMob.VitaminBannerId;
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        closeLb.IsVisible = true;
    }
}