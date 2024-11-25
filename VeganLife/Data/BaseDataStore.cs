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
            await this.InitAsync();
            return await ExecuteWithRetryAsync(async () =>
            {
                if (isUpdate || await IsExistingItem(item))
                {
                    return await UpdateItemAsync(item);
                }
                else
                {
                    return await AddItemAsync(item);
                }
            });
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
            // Treat Id == 0 as a new item that does not exist
            if (idValue is int intId && intId == 0)
            {
                return (false, default);
            }

            var goalItem = await this._connection.FindAsync<T>(idValue);
            return new(goalItem != null, goalItem);
        }

        // Add a new item
        private async Task<bool> AddItemAsync(T item)
        {
            return await _connection.InsertAsync(item) > 0;
        }

        // Update an existing item
        private async Task<bool> UpdateItemAsync(T item)
        {
            return await _connection.UpdateAsync(item) > 0;
        }

        // Centralized error handling
        private async Task<bool> ExecuteWithRetryAsync(Func<Task<bool>> operation)
        {
            try
            {
                return await operation();
            }
            catch (SQLiteException ex) when (ex.Result == SQLite3.Result.Error)
            {
                ex.LogError("SQLite error in ExecuteWithRetryAsync");
                await HandleDatabaseErrorAsync();
                return false;
            }
            catch (Exception ex)
            {
                ex.LogError("General error in ExecuteWithRetryAsync");
                return false;
            }
        }

        // Handle database errors (e.g., recreate table)
        private async Task HandleDatabaseErrorAsync()
        {
            await _connection.DropTableAsync<T>();
            await _connection.CreateTableAsync<T>();
        }
    }
}
