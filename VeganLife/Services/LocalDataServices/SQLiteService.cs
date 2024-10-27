// <copyright file="SQLiteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using SQLite;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.Services.LocalDataServices
{
    public class SQLiteService : ISQLite
    {
        private SQLiteAsyncConnection _sQLiteConnectionData = null;

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            if (this._sQLiteConnectionData != null)
            {
                return this._sQLiteConnectionData;
            }

            _sQLiteConnectionData = new SQLiteAsyncConnection(ConstantHelper.DatabasePath, ConstantHelper.SQLiteFlags);
            return _sQLiteConnectionData;
        }

        public async Task ApplyMigrationsAsync()
        {
            // Retrieve the current database version
            int currentVersion = await GetDatabaseVersionAsync();
            int newVersion = 1;

            if (currentVersion < newVersion)
            {
                await _sQLiteConnectionData.RunInTransactionAsync(async transaction =>
                {
                    if (currentVersion < newVersion)
                    {
                        await DropAllTablesAsync();
                        await SetDatabaseVersionAsync(newVersion);
                    }
                });
            }
        }

        private Task<int> GetDatabaseVersionAsync()
        {
            return _sQLiteConnectionData.ExecuteScalarAsync<int>("PRAGMA user_version");
        }

        private Task SetDatabaseVersionAsync(int version)
        {
            return _sQLiteConnectionData.ExecuteAsync($"PRAGMA user_version = {version}");
        }

        private async Task DropAllTablesAsync()
        {
            // Query all table names from the sqlite_master table
            var tableNames = await _sQLiteConnectionData.QueryScalarsAsync<string>("SELECT name FROM sqlite_master WHERE type='table'");

            foreach (var tableName in tableNames)
            {
                if (tableName == "sqlite_sequence")
                {
                    continue;
                }

                await _sQLiteConnectionData.ExecuteAsync($"DROP TABLE IF EXISTS {tableName}");
            }
        }
    }
}
