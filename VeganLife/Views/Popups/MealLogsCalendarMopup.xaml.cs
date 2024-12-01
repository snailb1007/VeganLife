using VeganLife.ViewModels.PopupViewModels;

namespace VeganLife.Views.Popups;

public partial class MealLogsCalendarMopup
{
	public MealLogsCalendarMopup(MealLogsMopupVM vm)
	{
		InitializeComponent();
        this.BindingContext = vm;
	}
}