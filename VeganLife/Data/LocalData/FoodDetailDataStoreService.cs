// <copyright file="FoodDetailDataStoreService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Models.FoodModel;
using VeganLife.Services.LocalDataServices;

namespace VeganLife.Data.LocalData
{
    public class FoodDetailDataStoreService : BaseDataStore<FoodDetailModel>
    {
        public FoodDetailDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
