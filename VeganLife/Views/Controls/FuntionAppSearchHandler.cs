using VeganLife.Helpers;
using VeganLife.Models.FoodModel;

namespace VeganLife.Views.Controls
{
    public class FuntionAppSearchHandler : SearchHandler
    {
        public bool IsSettingPage { private get; set; }

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

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                if (IsSettingPage)
                {
                    ItemsSource = (Shell.Current as AppShell).Routes
                        .Where(item => item.Key.ToLower().Contains(newValue.ToLower()));
                }
                else
                {
                    if (FoodPreviews?.Any() ?? false)
                        ItemsSource = FoodPreviews
                            .Where(f => f.Name.ToLower().Contains(newValue.ToLower()))
                            .ToList<FoodPreviewModel>();
                }

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
            await Task.Delay(500);
            if (IsSettingPage)
            {
                ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            }
            else
            {
                await ServicesHelper.GetService<INavigationService>().NavigateToFoodDetail(item);
            }
            // The following route works because route names are unique in this application.
            //await Shell.Current.GoToAsync($"{GetNavigationTarget()}?name={((Animal)item).Name}");
        }

        //string GetNavigationTarget()
        //{
        //    return (Shell.Current as AppShell).Routes.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
        //}
    }
}
