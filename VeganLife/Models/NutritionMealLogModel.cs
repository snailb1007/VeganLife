using SQLite;
using VeganLife.Models.CommunityFreeServiceModel;

namespace VeganLife.Models
{
    public class NutritionMealLogModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public USDAFoodNutritionFactModel Food { get; set; }

        public UndefinedMacroFoodNutriFactModel UndefinedFood { get; set; }

        public DateTime EatingDay { get; set; }

        public int Amount { get; set; }
    }
}
