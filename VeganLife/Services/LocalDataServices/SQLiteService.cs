using SQLite;

namespace VeganLife.Services.LocalDataServices
{
    public class SQLiteService : ISQLite
    {
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "SQLiteVeganLife.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}
