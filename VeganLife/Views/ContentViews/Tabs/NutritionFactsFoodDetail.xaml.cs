using CommunityToolkit.Maui.Views;

namespace VeganLife.Views.ContentViews.Tabs;

public partial class NutritionFactsFoodDetail : Expander
{
	public NutritionFactsFoodDetail()
	{
		InitializeComponent();
		this.IsExpanded = true;
	}
}