using VeganLife.Helpers.AppSetting;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class AthleticNutritionTab : ContentView
{
    public AthleticNutritionTab()
    {
        InitializeComponent();
        mtAdFixed.AdsId = ConstantHelper.GoogleAdMob.MacroBannerId;
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        closeLb.IsVisible = true;
    }
}