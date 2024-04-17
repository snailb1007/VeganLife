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
}