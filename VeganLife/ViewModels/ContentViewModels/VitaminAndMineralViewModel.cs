namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<VitaminModel> _vitamins;

        public VitaminAndMineralViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            Init();
        }

        async void Init()
        {
            Vitamins = await data_service.GetVitamins();
        }
    }
}
