// <copyright file="FuntionAppSearchHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Views.Controls
{
    using VeganLife.Helpers;
    using VeganLife.Models.FoodModel;
    using VeganLife.Views.FoodTab;

    public class FuntionAppSearchHandler : SearchHandler
    {
        public static BindableProperty FoodPreviewsProperty = BindableProperty.Create(
            propertyName: "FoodPreviews",
            declaringType: typeof(MainPage),
            returnType: typeof(IEnumerable<FoodPreviewModel>),
            defaultValue: default(IEnumerable<FoodPreviewModel>));

        public IEnumerable<FoodPreviewModel> FoodPreviews
        {
            get => (IEnumerable<FoodPreviewModel>)GetValue(FoodPreviewsProperty);
            set => SetValue(FoodPreviewsProperty, value);
        }

        public Type SelectedItemNavigationTarget { get; set; }

        public FuntionAppSearchHandler()
        {
            ClearPlaceholderCommand = new Command(() => this.Query = string.Empty);
        }

        private System.Timers.Timer typingTimer;
        private bool isSearching;

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            this.isSearching = true;
            this.typingTimer?.Dispose();
            this.typingTimer = new System.Timers.Timer(500);
            this.typingTimer.Elapsed += (s, arg) =>
            {
                if (this.isSearching)
                {
                    MainThread.BeginInvokeOnMainThread(() => this.DoSearch(newValue));
                    this.isSearching = false;
                }
            };
            this.typingTimer.Start();
        }

        private void DoSearch(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                this.ItemsSource = null;
            }
            else
            {

                if (this.FoodPreviews?.Any() ?? false)
                {
                    this.ItemsSource = this.FoodPreviews
                        .Where(f => f.Name.ToLower().Contains(newValue.ToLower()))
                        .ToList<FoodPreviewModel>();
                }
            }

            this.TextColor = ((IEnumerable<FoodPreviewModel>)this.ItemsSource)?.Any() ?? false ?
                Colors.SkyBlue : Colors.Red;
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (this.IsFocused)
            {
                this.Unfocus();
            }

            // Let the animation complete
            await Task.Delay(1);
            await ServicesHelper.GetService<INavigationService>().NavigataToPage<FoodDetailPage>(item);
        }
    }
}
