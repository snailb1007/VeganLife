using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    internal class UndefinedMacroFoodNutriFactDataStoreService : BaseDataStore<UndefinedMacroFoodNutriFactModel>
    {
        public UndefinedMacroFoodNutriFactDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
