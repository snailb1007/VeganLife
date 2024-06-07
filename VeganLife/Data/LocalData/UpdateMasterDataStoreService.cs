using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    internal class UpdateMasterDataStoreService : BaseDataStore<UpdateMasterModel>
    {
        public UpdateMasterDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
