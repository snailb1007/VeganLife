namespace VeganLife.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<FoodModel> _foods;

        public MainViewModel()
        {
        }


    }
}
