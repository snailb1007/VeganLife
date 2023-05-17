using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class FoodPreviewDataStoreService : BaseDataStore<FoodPreviewModel>
    {
        public FoodPreviewDataStoreService(ISQLite database) : base(database)
        {
        }
    }
}
