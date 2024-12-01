using SQLite;
using VeganLife.Models.BaseModel;

namespace VeganLife.Models
{
    public partial class NutritionMealLogModel : BaseFoodModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public int UsdaFoodId { get; set; }

        public string UndefinedFoodId { get; set; }

        public DateTime EatingDay { get; set; }

        public int Amount { get; set; }

        public bool IsRead {  get; set; }
    }
}
