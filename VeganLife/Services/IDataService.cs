using VeganLife.Models.FoodModel;

namespace VeganLife.Services
{
    public interface IDataService
    {
        Task<IEnumerable<FoodPreviewModel>> GetFoods();
        Task<FoodDetailModel> GetFoodDetail(string id);
        Task<IEnumerable<MenuModel>> GetFoodMenu();
        Task<IEnumerable<VitaminModel>> GetVitamins();
    }
}
