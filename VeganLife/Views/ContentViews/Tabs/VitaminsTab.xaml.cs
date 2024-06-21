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
        mtAdFixed.AdsId = ConstantHelper.GoogleAdMob.BannerId;
    }
}