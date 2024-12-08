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
            viewModel.NavigationViewModel = this.Navigation;
            FFImageLoading.Helpers.ServiceHelper.GetService<SentryService>().LogMessage($"★ {this.GetType()} created");
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

        public class BoolToOpacityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (bool)value ? 0.5 : 1;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}