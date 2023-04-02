namespace VeganLife.Views.Controls
{
    public class VitaminSearchHandler : SearchHandler
    {
        public static BindableProperty VitaminsProperty = BindableProperty.Create(
            propertyName: "Vitamins",
            declaringType: typeof(MainPage),
            returnType: typeof(IEnumerable<VitaminModel>),
            defaultValue: default(IEnumerable<VitaminModel>));
        public IEnumerable<VitaminModel> Vitamins
        {
            get => (IEnumerable<VitaminModel>)GetValue(VitaminsProperty);
            set => SetValue(VitaminsProperty, value);
        }

        public Type SelectedItemNavigationTarget { get; set; }

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

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            // Let the animation complete
            await Task.Delay(100);
        }

        void DoSearch(string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                if (Vitamins?.Any() ?? false)
                    ItemsSource = Vitamins
                        .Where(f => f.Name.ToLower().Contains(newValue.ToLower()))
                        .ToList<VitaminModel>();

            }

            TextColor = ((IEnumerable<VitaminModel>)ItemsSource)?.Any() ?? false ?
                Colors.SkyBlue : Colors.Red;
        }
    }
}
