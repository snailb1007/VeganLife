using VeganLife.Helpers.AppSetting;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class VitaminsTab
{
    public VitaminAndMineralViewModel ViewModel => (VitaminAndMineralViewModel)BindingContext;

    public VitaminsTab()
    {
        InitializeComponent();
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        closeLb.IsVisible = true;
    }

    private void MtAdFixedVitaminAndMineral_AdsFailedToLoad(object sender, Plugin.MauiMTAdmob.Extra.MTEventArgs e)
    {
        FFImageLoading.Helpers.ServiceHelper.GetService<SentryService>().LogMessage($"VitaminsTab AdsFailedToLoad\nCode: {e.ErrorCode} {e.ErrorMessage}");
    }

    private void ThisView_Loaded(object sender, EventArgs e)
    {
        SetupAdsBanner();
    }

    private void SetupAdsBanner()
    {
        mtAdFixedVitaminAndMineral.AdsId = ConstantHelper.GoogleAdMob.VitaminBannerId;
    }
}