using SQLite;
using VeganLife.Data.LocalData;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data
{
    public class BaseDataStore<T> : IDataStoreService<T>
        where T : new()
    {
        SQLiteAsyncConnection connection;
        readonly ISQLite localDatabase;

        public BaseDataStore(ISQLite database)
        {
            this.localDatabase = database;
        }

        private async Task Init()
        {
            if (this.connection is not null)
            {
                return;
            }

            this.connection = this.localDatabase.GetAsyncConnection();
            await this.connection?.CreateTableAsync<T>();
        }

        public async Task<bool> AddOrUpdateItemAsync(T item, bool isUpdate = false)
        {
            try
            {
                await Init();
                if (isUpdate)
                    await this.connection.UpdateAsync(item);
                else
                    await this.connection.InsertAsync(item);
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
            await this.Init();
            try
            {
                await this.connection.DeleteAsync(item);
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
            await this.Init();
            try
            {
                return await this.connection.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("Cant retrive local data, " + e.Message);
                return Enumerable.Empty<T>();
            }
        }
    }
}
