using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    internal class UsdaFoodPreviewsDataStore : BaseDataStore<USDAFoodPreviewModel>
    {
        public UsdaFoodPreviewsDataStore(ISQLite database)
            : base(database)
        {
        }
    }
}