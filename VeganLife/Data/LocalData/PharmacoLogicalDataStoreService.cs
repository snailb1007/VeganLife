using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    internal class PharmacoLogicalDataStoreService : BaseDataStore<PharmacoLogicalModel>
    {
        public PharmacoLogicalDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}