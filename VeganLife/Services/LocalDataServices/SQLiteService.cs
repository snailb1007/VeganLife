// <copyright file="SQLiteService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services.LocalDataServices
{
    using SQLite;
    using VeganLife.Helpers.AppSetting;

    public class SQLiteService : ISQLite
    {
        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return new SQLiteAsyncConnection(ConstantHelper.DatabasePath, ConstantHelper.SQLiteFlags);
        }
    }
}
