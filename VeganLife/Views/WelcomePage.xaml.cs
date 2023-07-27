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

        private async void NextButton(object sender, EventArgs e)
        {
            await Task.Delay(100);
            carouselView.ScrollTo(carouselView.Position + 1, position: ScrollToPosition.Center);
        }

        private async void PreviousButton(object sender, EventArgs e)
        {
            await Task.Delay(100);
            carouselView.ScrollTo(carouselView.Position - 1, position: ScrollToPosition.Center);
        }
    }
}