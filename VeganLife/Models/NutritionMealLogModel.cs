using Realms;
using VeganLife.Models.BaseModel;

namespace VeganLife.Models
{
    public partial class NutritionMealLogModel : BaseFoodModel
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public int UsdaFoodId { get; set; }

        public string UndefinedFoodId { get; set; }

        public DateTime EatingDay { get; set; }

        public int Amount { get; set; }

        public bool IsRead {  get; set; }
    }

    public partial class NutritionMealLogModel
    {
        [Ignored]
        public double DisplayCalories => this.Calories.Amount * this.Amount / 100f;
        [Ignored]
        public double DisplayProtein => this.Protein.Amount * this.Amount / 100f;
        [Ignored]
        public double DisplayFat => this.Fat.Amount * this.Amount / 100f;
        [Ignored]
        public double DisplayCarbs => this.Carbohydrate.Amount * this.Amount / 100f;

        public bool IsUsda => this.UsdaFoodId > 0;
        public bool IsUndefined => !string.IsNullOrEmpty(this.UndefinedFoodId);
    }
}
