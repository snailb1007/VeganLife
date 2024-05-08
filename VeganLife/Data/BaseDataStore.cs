// <copyright file="BaseDataStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SQLite;
using VeganLife.Data.LocalData;
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
        }

        /// <inheritdoc/>
        public async Task<bool> AddOrUpdateItemAsync(T item, bool isUpdate = false)
        {
            try
            {
                await this.Init();
                if (await this.IsExistingItem(item))
                {
                    await this._connection.UpdateAsync(item);
                }
                else
                {
                    await this._connection.InsertAsync(item);
                }

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                _ = e;
#if DEBUG
                await Console.Out.WriteLineAsync(e.Message);
#endif
                return await Task.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteItem(T item)
        {
            await this.Init();
            try
            {
                await this._connection.DeleteAsync(item);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return await Task.FromResult(false);
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetItemAsync()
        {
            await this.Init();
            return await this._connection.Table<T>().FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            await this.Init();
            try
            {
                return await this._connection.Table<T>().ToListAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("Cant retrieve local data, " + e.Message);
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
#if DEBUG
                await Console.Out.WriteLineAsync("Cant retrieve local data, " + e.Message);
#endif
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
            var idValue = idProperty?.GetValue(item);
            var goalItem = await this._connection.FindAsync<T>(idValue);
            return goalItem != null;
        }
    }
}
