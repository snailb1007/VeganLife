// <copyright file="IDataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services
{
    using VeganLife.Models.FirebaseDataModel;
    using VeganLife.Models.FoodModel;
    using VeganLife.Models.GoogleNewsModels;

    public interface IDataService
    {
        Task<IEnumerable<FoodPreviewModel>> GetFoods();

        Task<FoodDetailModel> GetFoodDetail(string id);

        Task<IEnumerable<FoodMenuCategoryModel>> GetFoodMenu();

        Task<FoodNutrientFacts> GetFoodNutriFacts(string id);

        Task<IEnumerable<VitaminModel>> GetVitamins();

        Task<IEnumerable<Item>> LoadGoogleNews(string uri);
        Task<List<string>> GetImageLinksAsync(string url);
        Task<string> GetFireBaseValue(string nodePath);
        Task<BmiModel> GetHealthDiagnosisFirebaseDataModel();
    }
}
