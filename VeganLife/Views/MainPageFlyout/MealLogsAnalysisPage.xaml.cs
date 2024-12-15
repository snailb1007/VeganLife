namespace VeganLife.Views.MainPageFlyout;

public partial class MealLogsAnalysisPage
{
	public MealLogsAnalysisPage(MealLogsAnalysisPageVM vm)
		: base(vm)
	{
		InitializeComponent();
		HandlerProperties.SetDisconnectPolicy(pieChart, HandlerDisconnectPolicy.Manual);
	}
}