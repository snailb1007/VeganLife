using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VeganLife.Models;

namespace VeganLife.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        IEnumerable<FoodModel> _foods;        

        public MainViewModel()
        {
        }

        
    }
}
