using SQLite;
using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Models
{
    public class NutritionMealLogModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public FoodNutrient Food { get; set; }

        public DateTime EatingDay { get; set; }
    }
}
