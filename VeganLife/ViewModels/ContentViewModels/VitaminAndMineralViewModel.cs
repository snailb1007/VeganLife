namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<VitaminModel> _vitamins;

        public VitaminAndMineralViewModel(IDataService dataService) : base(dataService)
        {
            Init();
        }

        async void Init()
        {
            Vitamins = await data_service.GetVitamins();
        }
    }
}
