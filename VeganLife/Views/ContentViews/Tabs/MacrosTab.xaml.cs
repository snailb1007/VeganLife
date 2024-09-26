using VeganLife.Helpers.AppSetting;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class MacrosTab : ContentView
{
    private MacrosViewModel _vm;
    private Timer _timer;

    public MacrosTab()
    {
        InitializeComponent();
        _timer = new Timer(this.ScrollTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        SetupAdsBanner();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        _vm ??= this.BindingContext as MacrosViewModel;
    }

    private void RefreshView_Refreshing(object sender, EventArgs e)
    {
        refreshView.IsRefreshing = false;
    }

    private void SetupAdsBanner()
    {
        mtAdFixed.AdsId = ConstantHelper.GoogleAdMob.MacroBannerId;
    }

    private void OnAdLoaded(object sender, EventArgs e)
    {
        closeLb.IsVisible = true;
    }

    private void FoodsPreviewCollection_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        _vm.IsScrolling = true;
        this._timer.Change(500, Timeout.Infinite);
    }

    private void ScrollTimerElapsed(object obj)
    {
        _vm.IsScrolling = false;
    }
}