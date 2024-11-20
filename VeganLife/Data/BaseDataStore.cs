// <copyright file="BaseDataStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AsyncAwaitBestPractices;
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
        private Task _currentInitTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDataStore{T}"/> class.
        /// </summary>
        /// <param name="database">ISQLite.</param>
        public BaseDataStore(ISQLite database)
        {
            this._localDatabase = database;
            _currentInitTask = InitAsync();
            _currentInitTask.SafeFireAndForget(ex => ex.LogError());
        }

        public async Task<bool> SaveItems(IEnumerable<T> items)
        {
            await this.InitAsync();
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
            await this.InitAsync();
            try
            {
                var res = await this._connection.DeleteAllAsync<T>();
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
                await this.InitAsync();
                if (isUpdate || await this.IsExistingItem(item))
                {
                    return await this._connection.UpdateAsync(item) > 0;
                }
                else
                {
                    Task<int> task;
                    task = this._connection.InsertOrReplaceAsync(item);
                    return (await task) > 0;
                }
            }
            catch (Exception e)
            {
                e.LogError();
                if (e is SQLiteException sqliteException
                    && sqliteException.Result == SQLite3.Result.Error)
                {
                    sqliteException.LogError("Error in AddOrUpdateItemAsync");
                    await this._connection.DropTableAsync<T>();
                    await this._connection.CreateTableAsync<T>();
                    var retry = await this._connection.InsertOrReplaceAsync(item);
                    return retry > 0;
                }
#if DEBUG
                throw new Exception("==> Error in AddOrUpdateItemAsync", e);
#else
                return false;
#endif
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteItem(T item)
        {
            await this.InitAsync();
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
        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var res = await this._connection.FindAsync<T>(id);
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
                var result = await this._connection.Table<T>().ToListAsync();
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
            await this.InitAsync();
            try
            {
                var result = await this._connection.Table<T>()
                    .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {
                e.LogError(description: "Cant retrieve local data");
                return default!;
            }
        }

        private async Task InitAsync()
        {
            if (this._currentInitTask != null && !this._currentInitTask.IsCompleted)
            {
                await _currentInitTask;
            }

            if (this._connection is not null)
            {
                return;
            }

            this._connection = this._localDatabase.GetAsyncConnection();
            await this._connection?.CreateTableAsync<T>()!;
        }

        public async Task<bool> IsExistingItem(T item)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty == null)
            {
                throw new InvalidOperationException("Type must have an 'Id' property.");
            }

            var result = await this.IsExistingItem(idValue: idProperty.GetValue(item));
            return result.isExised;
        }

        public async Task<(bool isExised, T result)> IsExistingItem(object idValue)
        {
            var goalItem = await this._connection.FindAsync<T>(idValue);
            return new (goalItem != null, goalItem);
        }
    }
}
