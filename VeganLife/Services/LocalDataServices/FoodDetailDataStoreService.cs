using VeganLife.Models.FoodModel;

namespace VeganLife.Services.LocalDataServices
{
    public class FoodDetailDataStoreService : IDataStoreService<FoodDetailModel>
    {
        public Task<bool> AddOrUpdateItemAsync(FoodDetailModel item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(string id)
        {
            throw new NotImplementedException();
        }

        public Task<FoodDetailModel> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FoodDetailModel>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }
    }
}
