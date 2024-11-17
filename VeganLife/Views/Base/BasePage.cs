// <copyright file="BasePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;

namespace VeganLife.Views.Base
{
    public abstract class BasePage<TViewModel> : BasePage
        where TViewModel : BaseViewModel
    {
        protected BasePage(TViewModel viewModel)
            : base(viewModel)
        {
            viewModel.NavigationViewModel = this.Navigation;
            ServicesHelper.GetService<SentryService>().LogMessage($"★ {this.GetType()} created");
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine($"★ OnAppearing: {this.Title}");
            this.Dispatcher.Dispatch(async () => await this.BindingContext?.ViewAppearingVM());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Debug.WriteLine($"★ OnDisappearing: {this.Title}");
            this.Dispatcher.Dispatch(async () => await this.BindingContext?.ViewDisappearingVM());
        }
    }

    public abstract class BasePage : ContentPage
    {
        protected BasePage(object viewModel = null)
        {
            this.BindingContext = viewModel;
        }

        //protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(backingStore, value))
        //    {
        //        return false;
        //    }

        //    backingStore = value;
        //    onChanged?.Invoke();
        //    this.OnPropertyChanged(propertyName);
        //    return true;
        //}

        //public event PropertyChangedEventHandler BasePagePropertyChanged;

        ///// <inheritdoc/>
        //protected override void OnPropertyChanged([CallerMemberName] string propertyName = $"")
        //{
        //    base.OnPropertyChanged(propertyName);
        //    var changed = this.BasePagePropertyChanged;

        //    changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}