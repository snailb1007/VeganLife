using SQLite;
using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Models
{
    public class NutritionMealLogModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        public USDAFoodNutritionFactModel Food { get; set; }

        public UndefinedMacroFoodNutriFactModel UndefinedFood { get; set; }

        public DateTime EatingDay { get; set; }
    }
}
