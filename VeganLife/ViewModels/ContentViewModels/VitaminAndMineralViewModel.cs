using VeganLife.Data.FireBaseData;

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class VitaminAndMineralViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<VitaminModel> _vitamins;

        public VitaminAndMineralViewModel()
        {
            Init();
        }

        async void Init()
        {
            //Vitamins = new IEnumerable<VitaminModel>();
            var data = await new FirebaseRealtimeData().GetVitamins();
            Vitamins = data;
        }
    }
}
