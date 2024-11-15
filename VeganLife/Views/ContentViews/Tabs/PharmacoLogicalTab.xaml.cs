using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class PharmacoLogicalTab
{
    public PharmacoLogicalTab()
    {
        InitializeComponent();
    }

    private void SetupAdsBanner()
    {
        mtAdFixedPharmacoLogical.AdsId = ConstantHelper.GoogleAdMob.VitaminBannerId;
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        closeLb.IsVisible = true;
    }

    private void MtAdFixedPharmacoLogical_AdsFailedToLoad(object sender, Plugin.MauiMTAdmob.Extra.MTEventArgs e)
    {
        ServicesHelper.GetService<SentryService>().LogMessage($"PharmacoLogicalTab AdsFailedToLoad\nCode: {e.ErrorCode} {e.ErrorMessage}");
    }
}