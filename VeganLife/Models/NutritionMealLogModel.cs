using SQLite;
using VeganLife.Helpers;
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

    public partial class NutritionMealLogModel
    {
        [Ignore]
        public double DisplayCalories => this.CaloriesAmount * this.Amount / 100f;
        [Ignore]
        public double DisplayProtein => this.ProteinAmount * this.Amount / 100f;
        [Ignore]
        public double DisplayFat => this.FatAmount * this.Amount / 100f;
        [Ignore]
        public double DisplayCarbs => this.CarbohydrateAmount * this.Amount / 100f;

        public bool IsUsda => this.UsdaFoodId > 0;
        public bool IsUndefined => !string.IsNullOrEmpty(this.UndefinedFoodId);
        public string FirstNameCharacter => string.IsNullOrEmpty(this.Name) ?
            "?"
            : this.Name.ToString()[0].ToString().ToUpper();

        //public string TimeAgo => DateTimeHelper.CalculateTimeAgo(this.EatingDay);
    }
}
