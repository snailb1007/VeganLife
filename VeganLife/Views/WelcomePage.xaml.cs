namespace VeganLife.Views
{
    using VeganLife.Views.Base;
    public partial class WelcomePage : BasePage<WelcomeViewModel>
    {
        public WelcomePage(WelcomeViewModel vm)
            : base(vm)
        {
            this.InitializeComponent();
        }
    }
}