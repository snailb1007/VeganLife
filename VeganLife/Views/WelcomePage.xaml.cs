using VeganLife.Views.Base;

namespace VeganLife.Views;

public partial class WelcomePage : BasePage<WelcomeViewModel>
{
    public WelcomePage(WelcomeViewModel vm) : base(vm)
	{
        if (carouselView != null)
            carouselView.Position = 1;
        InitializeComponent();
    }
}