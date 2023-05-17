using SQLite;
using VeganLife.Data.LocalData;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data
{
    public class BaseDataStore<T> : IDataStoreService<T> where T : new()
    {
        SQLiteAsyncConnection _connection;
        ISQLite _localDatabase;

        public BaseDataStore(ISQLite database)
        {
            _localDatabase = database;
        }
        async Task Init()
        {
            if (_connection is not null)
                return;
            _connection = _localDatabase.GetAsyncConnection();
            await _connection?.CreateTableAsync<T>();
        }
        public async Task<bool> AddOrUpdateItemAsync(T item, bool isUpdate = false)
        {
            try
            {
                await Init();
                if (isUpdate)
                    await _connection.UpdateAsync(item);
                else
                    await _connection.InsertAsync(item);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteItem(T item)
        {
            await Init();
            try
            {
                await _connection.DeleteAsync(item);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return await Task.FromResult(false);
            }
        }

        public Task<T> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            await Init();
            try
            {
                return await _connection.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("Cant retrive local data, " + e.Message);
                return Enumerable.Empty<T>();
            }
        }
    }
}
