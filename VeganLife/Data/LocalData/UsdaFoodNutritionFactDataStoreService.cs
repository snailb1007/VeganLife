using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class UsdaFoodNutritionFactDataStoreService : BaseDataStore<USDAFoodNutritionFactModel>
    {
        public UsdaFoodNutritionFactDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
