using VeganLife.Helpers.AppSetting;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class AthleticNutritionTab
{
    public AthleticNutritionTabVM ViewModel => BindingContext as AthleticNutritionTabVM;

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