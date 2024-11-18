using VeganLife.Helpers.AppSetting;
using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class MacrosTab
{
    private readonly Timer _timer;
    
    public MacrosViewModel ViewModel { get; private set; }

    public MacrosTab()
    {
        InitializeComponent();
        _timer = new Timer(this.ScrollTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
        SetupAdsBanner();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel ??= this.BindingContext as MacrosViewModel ?? throw new NullReferenceException();
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
        if (ViewModel is null)
        {
            return;
        }

        ViewModel.IsScrolling = true;
        _ = Task.Run(() => this._timer.Change(500, Timeout.Infinite));
    }

    private void ScrollTimerElapsed(object obj)
    {
        ViewModel.IsScrolling = false;
    }
}