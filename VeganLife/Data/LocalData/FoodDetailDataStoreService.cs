// <copyright file="FoodDetailDataStoreService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Data.LocalData
{
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.LocalDataServices;

    public class FoodDetailDataStoreService : BaseDataStore<FoodDetailModel>
    {
        public FoodDetailDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
