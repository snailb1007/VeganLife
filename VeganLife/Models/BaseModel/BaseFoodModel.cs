using Newtonsoft.Json;
using SQLite;

namespace VeganLife.Models.BaseModel
{
    public abstract class BaseFoodModel
    {
        [JsonIgnore]
        public BaseMacroFoodFactModel Calories { get; set; } = new();

        [JsonIgnore]
        public BaseMacroFoodFactModel Carbohydrate { get; set; } = new();

        [JsonIgnore]
        public BaseMacroFoodFactModel Fat { get; set; } = new();

        [JsonIgnore]
        public BaseMacroFoodFactModel Protein { get; set; } = new();

    }

    public class BaseMacroFoodFactModel
    {
        public double Amount { get; set; } = -1;

        public string Unit { get; set; } = string.Empty;
    }
}
