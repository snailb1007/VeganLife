namespace VeganLife.Data.LocalData
{
    public interface IDataStoreService<T>
    {
        Task<bool> AddOrUpdateItemAsync(T item);
        Task<bool> DeleteItem(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
