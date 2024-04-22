using VeganLife.ViewModels.TabsViewModel;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class MacrosTab : ContentView
{
	public MacrosTab()
	{
		InitializeComponent();
	}

    private void RefreshView_Refreshing(object sender, EventArgs e)
    {
        refreshView.IsRefreshing = false;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        (this.BindingContext as MacrosViewModel)?.ItemSelectedChangedCommand
            .Execute((sender as View)?.BindingContext);
    }
}