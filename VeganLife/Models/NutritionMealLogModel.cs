using SQLite;
using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Models
{
    public class NutritionMealLogModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int UsdaFoodId { get; set; }

        public string UndefinedFoodId { get; set; }

        public DateTime EatingDay { get; set; }

        public int Amount { get; set; }
    }
}
