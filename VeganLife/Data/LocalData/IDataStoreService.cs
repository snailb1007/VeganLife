namespace VeganLife.Data.LocalData
{
    public interface IDataStoreService<T>
    {
        Task<bool> AddOrUpdateItemAsync(T item, bool isUpdate = false);
        Task<bool> DeleteItem(T item);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
