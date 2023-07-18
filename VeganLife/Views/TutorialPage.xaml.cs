using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class TutorialPage : BasePage<TutorialViewModel>
{
	public TutorialPage(TutorialViewModel vm)
        : base(vm)
    {
		this.InitializeComponent();
	}
}