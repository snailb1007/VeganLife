using VeganLife.Helpers.AppSetting;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Resources.Translations;

namespace VeganLife.Helpers
{
    internal class NutritionFactsHelper
    {
        internal static readonly Dictionary<string, string> VietnameseTranslations = new Dictionary<string, string>
        {
            { ConstantHelper.UsdaFoodNutrition.Proximates, AppResources.Proximates_common },
            { ConstantHelper.UsdaFoodNutrition.Water, AppResources.Water_common },
            { ConstantHelper.UsdaFoodNutrition.Energy, AppResources.Energy_common },
            { ConstantHelper.UsdaFoodNutrition.Protein, AppResources.Protein_common },
            { ConstantHelper.UsdaFoodNutrition.Fat, AppResources.Fat_common },
            { ConstantHelper.UsdaFoodNutrition.Ash, AppResources.Ash_common },
            { ConstantHelper.UsdaFoodNutrition.Carbohydrate, AppResources.Carbohydrate_common },
            { ConstantHelper.UsdaFoodNutrition.Fiber, AppResources.Fiber_common },
            { ConstantHelper.UsdaFoodNutrition.Sugars, AppResources.Sugars_common },
            { ConstantHelper.UsdaFoodNutrition.Sucrose, AppResources.Sucrose_common },
            { ConstantHelper.UsdaFoodNutrition.Glucose, AppResources.Glucose_common },
            { ConstantHelper.UsdaFoodNutrition.Fructose, AppResources.Fructose_common },
            { ConstantHelper.UsdaFoodNutrition.Lactose, AppResources.Lactose_common },
            { ConstantHelper.UsdaFoodNutrition.Maltose, AppResources.Maltose_common },
            { ConstantHelper.UsdaFoodNutrition.GaLactose, AppResources.GaLactose_common },
            { ConstantHelper.UsdaFoodNutrition.Starch, AppResources.Starch_common },
            { ConstantHelper.UsdaFoodNutrition.Minerals, AppResources.Minerals_common },
            { ConstantHelper.UsdaFoodNutrition.Calcium, AppResources.Calcium_common },
            { ConstantHelper.UsdaFoodNutrition.Iron, AppResources.Iron_common },
            { ConstantHelper.UsdaFoodNutrition.Magnesium, AppResources.Magnesium_common },
            { ConstantHelper.UsdaFoodNutrition.Phosphorus, AppResources.Phosphorus_common },
            { ConstantHelper.UsdaFoodNutrition.Potassium, AppResources.Potassium_common },
            { ConstantHelper.UsdaFoodNutrition.Sodium, AppResources.Sodium_common },
            { ConstantHelper.UsdaFoodNutrition.Zinc, AppResources.Zinc_common },
            { ConstantHelper.UsdaFoodNutrition.Copper, AppResources.Copper_common },
            { ConstantHelper.UsdaFoodNutrition.Manganese, ConstantHelper.UsdaFoodNutrition.Manganese },
            { ConstantHelper.UsdaFoodNutrition.Selenium, ConstantHelper.UsdaFoodNutrition.Selenium },
            { ConstantHelper.UsdaFoodNutrition.VitaminC, ConstantHelper.UsdaFoodNutrition.VitaminC },
            { ConstantHelper.UsdaFoodNutrition.Thiamin, ConstantHelper.UsdaFoodNutrition.Thiamin },
            { ConstantHelper.UsdaFoodNutrition.Riboflavin, ConstantHelper.UsdaFoodNutrition.Riboflavin },
            { ConstantHelper.UsdaFoodNutrition.Niacin, ConstantHelper.UsdaFoodNutrition.Niacin },
            { ConstantHelper.UsdaFoodNutrition.PantothenicAcid, ConstantHelper.UsdaFoodNutrition.PantothenicAcid },
            { ConstantHelper.UsdaFoodNutrition.VitaminB6, ConstantHelper.UsdaFoodNutrition.VitaminB6 },
            { ConstantHelper.UsdaFoodNutrition.Folate, ConstantHelper.UsdaFoodNutrition.Folate },
            { ConstantHelper.UsdaFoodNutrition.VitaminB12, ConstantHelper.UsdaFoodNutrition.VitaminB12 },
            { ConstantHelper.UsdaFoodNutrition.VitaminA, ConstantHelper.UsdaFoodNutrition.VitaminA },
            { ConstantHelper.UsdaFoodNutrition.VitaminE, ConstantHelper.UsdaFoodNutrition.VitaminE },
            { ConstantHelper.UsdaFoodNutrition.VitaminD, ConstantHelper.UsdaFoodNutrition.VitaminD },
            { ConstantHelper.UsdaFoodNutrition.VitaminK, ConstantHelper.UsdaFoodNutrition.VitaminK },
        };
        internal static void SetNutrientValue(ref UndefinedFoodNutrient targetNutrient, FoodNutrient source,
                    string[] searchTerms)
        {
            var nutrientName = source.Nutrient?.Name;
            var isMatchesAllTerms = searchTerms
                .All(term =>
                    !string.IsNullOrEmpty(nutrientName) &&
                    nutrientName.Contains(term, StringComparison.OrdinalIgnoreCase));
            if (targetNutrient == null && isMatchesAllTerms)
            {
                targetNutrient = new UndefinedFoodNutrient()
                {
                    Amount = source.Amount,
                    Unit = source?.Nutrient?.UnitName ?? string.Empty,
                };
            }
        }

        internal static void SetupUndefinedFood(UndefinedMacroFoodNutriFactModel data)
        {
            byte count = 0;
            foreach (var i in data?.foodNutrients!)
            {
                if (i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Protein, StringComparison.OrdinalIgnoreCase))
                {
                    data.ProteinAmount = i.Amount.Value;
                    data.ProteinUnit = i.Unit;
                    count++;
                }
                else if (i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Carbohydrate, StringComparison.OrdinalIgnoreCase))
                {
                    data.CarbohydrateAmount = i.Amount.Value;
                    data.CarbohydrateUnit = i.Unit;
                    count++;
                }
                else if (i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Energy, StringComparison.OrdinalIgnoreCase))
                {
                    data.CaloriesAmount = i.Amount.Value;
                    data.CaloriesUnit = i.Unit;
                    count++;
                }
                else if (i.Nutrient.Name.Contains(ConstantHelper.UsdaFoodNutrition.Fat, StringComparison.OrdinalIgnoreCase))
                {
                    data.FatAmount = i.Amount.Value;
                    data.FatUnit = i.Unit;
                    count++;
                }

                if (count == 4)
                {
                    break;
                }
            }
        }
    }
}
