using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class ReportPage : BasePage<ReportPageViewModel>
{
	public ReportPage(ReportPageViewModel vm)
		: base(vm)
	{
		this.InitializeComponent();
	}
}