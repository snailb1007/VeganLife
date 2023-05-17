namespace VeganLife.Services.LocalDataServices
{
    public interface ISQLite
    {
        SQLite.SQLiteAsyncConnection GetAsyncConnection();
    }
}
