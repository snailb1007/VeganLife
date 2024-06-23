using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class GoogleAdValidatorDataStoreService : BaseDataStore<GoogleAdValidatorModel>
    {
        public GoogleAdValidatorDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}