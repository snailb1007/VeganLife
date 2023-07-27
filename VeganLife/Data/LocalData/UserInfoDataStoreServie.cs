using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    class UserInfoDataStoreServie : BaseDataStore<UserInfo>
    {
        public UserInfoDataStoreServie(ISQLite database) : base(database)
        {
        }
    }
}
