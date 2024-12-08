using Newtonsoft.Json;

namespace VeganLife.Models.BaseModel
{
    public abstract class BaseFoodModel
    {
        [JsonIgnore]
        public double CaloriesAmount { get; set; }
        [JsonIgnore]
        public string CaloriesUnit { get; set; }

        [JsonIgnore]
        public double CarbohydrateAmount { get; set; }
        [JsonIgnore]
        public string CarbohydrateUnit { get; set; }

        [JsonIgnore]
        public double FatAmount { get; set; }
        [JsonIgnore]
        public string FatUnit { get; set; }

        [JsonIgnore]
        public double ProteinAmount { get; set; }
        [JsonIgnore]
        public string ProteinUnit { get; set; }

    }

    //public class BaseMacroFoodFactModel
    //{
    //    public float Amount { get; set; } = -1;

    //    public string Unit { get; set; } = string.Empty;
    //}
}
