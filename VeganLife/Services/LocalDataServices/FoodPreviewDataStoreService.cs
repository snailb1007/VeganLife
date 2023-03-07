using SQLite;
using VeganLife.Models.FoodModel;

namespace VeganLife.Services.LocalDataServices
{
    public class FoodPreviewDataStoreService : IDataStoreService<FoodPreviewModel>
    {
        SQLiteAsyncConnection _connection;

        public FoodPreviewDataStoreService(ISQLite db)
        {
            _connection  = db.GetAsyncConnection();
            _connection.CreateTableAsync<FoodPreviewModel>();
        }

        public async Task<bool> AddOrUpdateItemAsync(FoodPreviewModel item)
        {
            try
            {
                await _connection.InsertAsync(item);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return await Task.FromResult(false);
            }
        }

        public Task<bool> DeleteItem(string id)
        {
            throw new NotImplementedException();
        }

        public Task<FoodPreviewModel> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FoodPreviewModel>> GetItemsAsync(bool forceRefresh = false)
            => await _connection.Table<FoodPreviewModel>().ToListAsync();
    }
}
