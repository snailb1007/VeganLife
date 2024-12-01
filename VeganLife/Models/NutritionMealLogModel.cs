using SQLite;
using VeganLife.Helpers.AppSetting;
using VeganLife.Helpers;
using VeganLife.Models.BaseModel;
using VeganLife.Data.LocalData;
using VeganLife.Models.CommunityFreeServiceModel;

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
        public bool IsMayBeFailedData => this.Calories < 0 || this.Protein < 0 || this.Fat < 0 || this.Carbohydrate < 0;
        [Ignore]
        public double DisplayCalories => this.Calories * this.Amount / 100f;
        [Ignore]
        public double DisplayProtein => this.Protein * this.Amount / 100f;
        [Ignore]
        public double DisplayFat => this.Fat * this.Amount / 100f;
        [Ignore]
        public double DisplayCarbs => this.Carbohydrate * this.Amount / 100f;

        public bool IsUsda => this.UsdaFoodId > 0;
        public bool IsUndefined => !string.IsNullOrEmpty(this.UndefinedFoodId);

        [RelayCommand]
        private async Task RepairDataAsync()
        {
            UndefinedFoodNutrient caloriesValue = null;
            UndefinedFoodNutrient proteinValue = null;
            UndefinedFoodNutrient carbValue = null;
            UndefinedFoodNutrient fatValue = null;

            if (this.IsUsda)
            {
                var x = await FFImageLoading.Helpers.ServiceHelper.GetService<LocalDataStoreFactory>().GetDataStore<USDAFoodNutritionFactModel>().GetItemAsync(this.UsdaFoodId.ToString());
                foreach (var i in x.foodNutrients)
                {
                    NutritionFactsHelper.SetNutrientValue(ref proteinValue, i, [ConstantHelper.UsdaFoodNutrition.Protein]);
                    NutritionFactsHelper.SetNutrientValue(ref carbValue, i, [ConstantHelper.UsdaFoodNutrition.Carbohydrate, "difference"]);
                    NutritionFactsHelper.SetNutrientValue(ref caloriesValue, i, [ConstantHelper.UsdaFoodNutrition.Energy]);
                    NutritionFactsHelper.SetNutrientValue(ref fatValue, i, [ConstantHelper.UsdaFoodNutrition.Fat]);

                    if (proteinValue != null
                        && carbValue != null
                        && caloriesValue != null
                        && fatValue != null)
                    {
                        break;
                    }
                }
            }
        }
    }
}
