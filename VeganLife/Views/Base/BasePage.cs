namespace VeganLife.Views.Base
{
    public abstract class BasePage<TViewModel> : BasePage where TViewModel : BaseViewModel
    {
        protected BasePage(TViewModel viewModel) : base(viewModel)
        {
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;
    }

    public abstract class BasePage : ContentPage
    {
        protected BasePage(object viewModel = null)
        {
            BindingContext = viewModel;
            if (string.IsNullOrWhiteSpace(Title))
            {
                Title = GetType().Name;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
#if DEBUG
            Debug.WriteLine($"=> OnAppearing: {Title}");
#endif
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
#if DEBUG
            Debug.WriteLine($"=> OnDisappearing: {Title}");
#endif
        }
    }
}
