// <copyright file="BasePage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Base
{
    public abstract class BasePage<TViewModel> : BasePage
        where TViewModel : BaseViewModel
    {
        protected BasePage(TViewModel viewModel)
            : base(viewModel)
        {
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;
    }

    public abstract class BasePage : ContentPage, INotifyPropertyChanged
    {
        protected BasePage(object? viewModel = null)
        {
            this.BindingContext = viewModel;
            //if (string.IsNullOrWhiteSpace(this.Title))
            //{
            //    this.Title = this.GetType().Name;
            //}
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
#if DEBUG
            Debug.WriteLine($"=> OnAppearing: {this.Title}");
#endif
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await (this.BindingContext as BaseViewModel)?.ViewAppearingVM()!;
            });
        }

        /// <inheritdoc/>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
#if DEBUG
            Debug.WriteLine($"=> OnDisappearing: {this.Title}");
#endif
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await (this.BindingContext as BaseViewModel)?.ViewDisappearingVM()!;
            });
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler BasePagePropertyChanged;

        /// <inheritdoc/>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            var changed = this.BasePagePropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
