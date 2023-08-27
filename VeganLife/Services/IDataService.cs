// <copyright file="IDataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    using VeganLife.Models.FoodModel;
    using VeganLife.Models.GoogleNewsModels;

    public interface IDataService
    {
        Task<IEnumerable<FoodPreviewModel>> GetFoods();

        Task<FoodDetailModel> GetFoodDetail(string id);

        Task<IEnumerable<FoodMenuCategoryModel>> GetFoodMenu();

        Task<FoodNutriFacts> GetFoodNutriFacts(string id);

        Task<IEnumerable<VitaminModel>> GetVitamins();

        Task<IEnumerable<Item>> LoadGoogleNews(string uri);
    }
}
