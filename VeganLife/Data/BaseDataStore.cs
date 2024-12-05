// <copyright file="BaseDataStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Realms;
using VeganLife.Data.LocalData;
using VeganLife.Helpers.Extensions;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data
{
    /// <summary>
    /// Local storage using sqlite.
    /// </summary>
    public class BaseDataStore<T> : IDataStoreService<T>
        where T : RealmObject, new()
    {
        private readonly Realm _realm;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataStore{T}"/> class.
        /// </summary>
        /// <param name="database">ISQLite.</param>
        public BaseDataStore(ISQLite database)
        {
            //this._localDatabase = database;
            //_currentInitTask = InitAsync();
            //_currentInitTask.SafeFireAndForget(ex => ex.LogError());

            _realm = Realm.GetInstance();
        }

        public async Task<bool> SaveItems(IEnumerable<T> items)
        {
            try
            {
                await _realm.WriteAsync(() =>
                {
                    foreach (var item in items)
                    {
                        _realm.Add(item, update: true); // Upsert
                    }
                });
                return true;
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
        }

        public async Task<bool> DeleteAllItems()
        {
            try
            {
                await _realm.WriteAsync(() =>
                {
                    var allItems = _realm.All<T>();
                    _realm.RemoveRange(allItems);
                });
                return true;
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> AddOrUpdateItemAsync(T item, bool isUpdate = false)
        {
            try
            {
                await _realm.WriteAsync(() =>
                {
                    var ensureUpdate = isUpdate || IsExistingItem(item);
                    Debug.WriteLine("Realm update: " + ensureUpdate);
                    _realm.Add(item, update: ensureUpdate);
                });
                return true;
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteItem(T item)
        {
            try
            {
                await _realm.WriteAsync(() =>
                {
                    _realm.Remove(item);
                });
                return true;
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                await Task.Delay(0);
                var res = _realm.Find<T>(id);
                return res;
            }
            catch (Exception e)
            {
                e.LogError();
                return default!;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                await Task.Delay(0);
                var result = _realm.All<T>()?.ToList() ?? Enumerable.Empty<T>();
                return result;
            }
            catch (Exception e)
            {
                e.LogError();
                return Enumerable.Empty<T>();
            }
        }

        public async Task<T> GetFirstOrDefaultItem()
        {
            var res = (await this.GetItemsAsync()).FirstOrDefault();
            return res;
        }

        public bool IsExistingItem(T item)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Type must have an 'Id' property.");
            }

            var result = this.IsExistingItem(idValue: idProperty.GetValue(item));
            return result.isExised;
        }

        public (bool isExised, T result) IsExistingItem(object idValue)
        {
            T result = default;
            // Treat Id == 0 as a new item that does not exist
            if (idValue is int intId && intId == 0)
            {
                if (intId == 0)
                {
                    return (false, default);
                }

                result = _realm.Find<T>(intId);
            }

            result = _realm.Find<T>(idValue.ToString());
            return new(result != null, result);
        }

        //public bool AddOrUpdateItem(T item, bool isUpdate = false)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IDataStoreService<T>.SaveItems(IEnumerable<T> items)
        //{
        //    throw new NotImplementedException();
        //}

        //bool IDataStoreService<T>.DeleteAllItems()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<(bool isExised, T result)> IDataStoreService<T>.IsExistingItem(object idValue)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
