using Newtonsoft.Json;

namespace VeganLife.Models.BaseModel
{
    public abstract class BaseFoodModel
    {
        [JsonIgnore]
        public double Calories { get; set; } = -1;

        [JsonIgnore]
        public double Carbohydrate { get; set; } = -1;

        [JsonIgnore]
        public double Fat { get; set; } = -1;

        [JsonIgnore]
        public double Protein { get; set; } = -1;
    }
}
