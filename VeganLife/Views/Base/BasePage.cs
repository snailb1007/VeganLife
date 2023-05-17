namespace VeganLife.Views.Base
{
    public abstract class BasePage<TViewModel> : BasePage where TViewModel : BaseViewModel
    {
        protected BasePage(TViewModel viewModel) : base(viewModel)
        {
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;
    }

    public abstract class BasePage : ContentPage, INotifyPropertyChanged
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

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler BasePagePropertyChanged;
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            var changed = BasePagePropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
