// <copyright file="IDataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Models.FirebaseDataModel;
using VeganLife.Models.FoodModel;
using VeganLife.Models.GoogleNewsModels;

namespace VeganLife.Services
{
    public interface IDataService
    {
        Task<IEnumerable<FoodPreviewModel>> GetFoods();

        Task<FoodDetailModel> GetFoodDetail(string id);

        Task<IEnumerable<FoodMenuCategoryModel>> GetFoodMenu();

        Task<FoodNutrientFacts> GetFoodNutriFacts(string id);

        Task<IEnumerable<VitaminModel>> GetVitamins();

        IEnumerable<Item> ReadRssFeed(string url);

        Task<List<string>> GetImageLinksAsync(string url);

        Task<string> GetFireBaseValue(string nodePath);

        Task<BmiModel> GetHealthDiagnosisFirebaseDataModel();

        Task<IEnumerable<USDAFoodPreviewModel>> GetFoodsUSDA();

        Task<UndefinedMacroFoodNutriFactModel> GetMacroFoodNutriFacts(string id);
    }
}
