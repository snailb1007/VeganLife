// <copyright file="BaseDataStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AndroidX.Room;
using SQLite;
using VeganLife.Data.LocalData;
using VeganLife.Helpers.Extensions;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data
{
    /// <summary>
    /// Local storage using sqlite.
    /// </summary>
    public class BaseDataStore<T> : IDataStoreService<T>
        where T : new()
    {
        private readonly ISQLite _localDatabase;
        private SQLiteAsyncConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataStore{T}"/> class.
        /// </summary>
        /// <param name="database">ISQLite.</param>
        public BaseDataStore(ISQLite database)
        {
            this._localDatabase = database;
            Init().ConfigureAwait(false);
        }

        public async Task<bool> SaveItems(IEnumerable<T> items)
        {
            await this.Init();
            try
            {
                var res = await this._connection.InsertAllAsync(items);
                return res > 0;
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
        }

        public async Task<bool> DeleteAllItems()
        {
            await this.Init();
            try
            {
                var res = await this._connection.DeleteAllAsync<T>();
                await this._connection.DropTableAsync<T>();
                return res > 0;
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
                if (await this.IsExistingItem(item))
                {
                    return await this._connection.UpdateAsync(item) > 0;
                }
                else
                {
                    return await this._connection.InsertAsync(item) > 0;
                }
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
            await this.Init();
            try
            {
                return await this._connection.DeleteAsync(item) > 0;
            }
            catch (Exception e)
            {
                e.LogError();
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetItemAsync()
        {
            try
            {
                return await this._connection.Table<T>().FirstOrDefaultAsync();
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
                // select * from "VitaminModel" where "Id" = ?
                var tables = await _connection.GetTableInfoAsync("VitaminModel");
                foreach (var table in tables)
                {
                    Debug.WriteLine($"Column Name: {table.Name}, Column Type: {table.GetType()}");
                }

                return await this._connection.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                e.LogError();
                return Enumerable.Empty<T>();
            }
        }

        public async Task<T> GetFirstOrDefaultItem()
        {
            await this.Init();
            try
            {
                var result = await this._connection.Table<T>().ToListAsync()
                    .ContinueWith(t => t.Result.FirstOrDefault());
                return result ?? default!;
            }
            catch (Exception e)
            {
                _ = e;
                Debug.WriteLine("Cant retrieve local data, " + e.Message);
                return default!;
            }
        }

        private async Task Init()
        {
            if (this._connection is not null)
            {
                return;
            }

            this._connection = this._localDatabase.GetAsyncConnection();
            await this._connection?.CreateTableAsync<T>()!;
        }

        private async Task<bool> IsExistingItem(T item)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Type must have an 'Id' property.");
            }

            var idValue = idProperty?.GetValue(item);
            var goalItem = await this._connection.FindAsync<T>(idValue);
            return goalItem != null;
        }
    }
}
