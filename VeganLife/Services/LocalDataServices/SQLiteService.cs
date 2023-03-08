using SQLite;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.Services.LocalDataServices
{
    public class SQLiteService : ISQLite
    {
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(ConstantHelper.DatabasePath, ConstantHelper.SQLiteFlags);
        }
    }
}
