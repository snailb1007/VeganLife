using VeganLife.Helpers;
using VeganLife.Models.FoodModel;
using VeganLife.Views.FoodTab;

namespace VeganLife.Views.Controls
{
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
            ClearPlaceholderCommand = new Command(() => Query = string.Empty);
        }

        System.Timers.Timer _typingTimer;
        bool _isSearching;
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);
            _isSearching = true;
            _typingTimer?.Dispose();
            _typingTimer = new System.Timers.Timer(500);
            _typingTimer.Elapsed += (s, arg) =>
            {
                if (_isSearching)
                {
                    MainThread.BeginInvokeOnMainThread(() => DoSearch(newValue));
                    _isSearching = false;
                }
            };
            _typingTimer.Start();
        }

        void DoSearch(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {

                if (FoodPreviews?.Any() ?? false)
                    ItemsSource = FoodPreviews
                        .Where(f => f.Name.ToLower().Contains(newValue.ToLower()))
                        .ToList<FoodPreviewModel>();

            }

            TextColor = ((IEnumerable<FoodPreviewModel>)ItemsSource)?.Any() ?? false ?
                Colors.SkyBlue : Colors.Red;
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (this.IsFocused)
                this.Unfocus();
            // Let the animation complete
            await Task.Delay(1);
            await ServicesHelper.GetService<INavigationService>().NavigataToPage<FoodDetailPage>(item);
        }
    }
}
