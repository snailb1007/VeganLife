using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    internal class AffiliationDataStoreService : BaseDataStore<AffiliationModel>
    {
        public AffiliationDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
