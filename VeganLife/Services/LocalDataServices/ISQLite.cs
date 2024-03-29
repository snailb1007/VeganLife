// <copyright file="ISQLite.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services.LocalDataServices
{
    public interface ISQLite
    {
        SQLite.SQLiteAsyncConnection GetAsyncConnection();
    }
}
