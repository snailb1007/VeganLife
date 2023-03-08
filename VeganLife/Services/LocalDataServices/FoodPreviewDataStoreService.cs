using SQLite;
using VeganLife.Helpers;
using VeganLife.Helpers.AppSetting;
using VeganLife.Models.FoodModel;

namespace VeganLife.Services.LocalDataServices
{
    public class FoodPreviewDataStoreService : IDataStoreService<FoodPreviewModel>
    {
        SQLiteAsyncConnection _connection;

        public FoodPreviewDataStoreService()
        {
        }

        async Task Init()
        {
            if (_connection is not null)
                return;
            _connection = new SQLiteAsyncConnection(ConstantHelper.DatabasePath, ConstantHelper.SQLiteFlags);
            await _connection?.CreateTableAsync<FoodPreviewModel>();
        }

        public async Task<bool> AddOrUpdateItemAsync(FoodPreviewModel item)
        {
            try
            {
                await Init();
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
        {
            await Init();
            return await _connection.Table<FoodPreviewModel>().ToListAsync();
        }
    }
}
