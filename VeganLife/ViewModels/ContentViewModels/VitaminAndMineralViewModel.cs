namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<VitaminModel> _vitamins;

        public VitaminAndMineralViewModel(INavigationService navigationService, IDataService dataService) : base(navigationService, dataService)
        {
            Init();
            LoadDataAsync().ConfigureAwait(false);
        }

        void Init()
        {
        }

        async Task LoadDataAsync()
        {
            Vitamins = await data_service.GetVitamins();
        }

        [RelayCommand]
        async Task ItemSelectedAsync()
        {
            await Task.Delay(1);
        }
    }
}
