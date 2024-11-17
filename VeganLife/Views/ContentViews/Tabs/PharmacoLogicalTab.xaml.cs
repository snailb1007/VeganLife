using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class PharmacoLogicalTab
{
    public PharmacoLogicalTabVM ViewModel { get; private set; }

    public PharmacoLogicalTab()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel = BindingContext as PharmacoLogicalTabVM;
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