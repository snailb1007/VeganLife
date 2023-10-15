using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class UsdaFoodDataStoreService : BaseDataStore<USDAFoodNutritionFactModel>
    {
        public UsdaFoodDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
