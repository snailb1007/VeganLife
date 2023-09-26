using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class ReportPage : BasePage<ReportPageViewModel>
{
	public ReportPage(ReportPageViewModel vm)
		: base(vm)
	{
		this.InitializeComponent();
	}

    private void UnderlinedTabItem_Focused(object sender, FocusEventArgs e)
    {
        Console.WriteLine("==> UnderlinedTabItem_Focused ");
    }
}