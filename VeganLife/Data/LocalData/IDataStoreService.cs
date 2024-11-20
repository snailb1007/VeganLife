// <copyright file="IDataStoreService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Data.LocalData
{
    public interface IDataStoreService<T>
    {
        Task<bool> AddOrUpdateItemAsync(T item, bool isUpdate = false);

        Task<bool> DeleteItem(T item);

        Task<T> GetItemAsync(string id);

        Task<T> GetFirstOrDefaultItem();

        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task<bool> SaveItems(IEnumerable<T> items);

        Task<bool> DeleteAllItems();

        Task<bool> IsExistingItem(T item);

        Task<(bool isExised, T result)> IsExistingItem(object idValue);
    }
}
