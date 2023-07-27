// <copyright file="FoodPreviewDataStoreService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Data.LocalData
{
    using VeganLife.Models.FoodModel;
    using VeganLife.Services.LocalDataServices;

    public class FoodPreviewDataStoreService : BaseDataStore<FoodPreviewModel>
    {
        public FoodPreviewDataStoreService(ISQLite database)
            : base(database)
        {
        }
    }
}
