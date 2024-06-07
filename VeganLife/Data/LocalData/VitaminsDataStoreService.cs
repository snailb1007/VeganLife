using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    internal class VitaminsDataStoreService : BaseDataStore<VitaminModel>
    {
        public VitaminsDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
