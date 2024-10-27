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
            ServicesHelper.GetService<SentryService>().LogMessage($"★ {this.GetType()} created");
        }

        public new TViewModel BindingContext => (TViewModel)base.BindingContext;
    }

    public abstract class BasePage : ContentPage, INotifyPropertyChanged
    {
        protected BasePage(object? viewModel = null)
        {
            this.BindingContext = viewModel;
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine($"★ OnAppearing: {this.Title}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var vm = this.BindingContext as BaseViewModel;
                if (vm is null)
                {
                    return;
                }

                await vm.ViewAppearingVM()!;
            });
        }

        /// <inheritdoc/>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Debug.WriteLine($"★ OnDisappearing: {this.Title}");
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await (this.BindingContext as BaseViewModel)?.ViewDisappearingVM()!;
            });
        }

        protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
        {
            Debug.WriteLine($"★ OnNavigatedFrom: {this.Title}");
            base.OnNavigatedFrom(args);
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