using Newtonsoft.Json;

namespace VeganLife.Models.BaseModel
{
    public abstract class BaseFoodModel
    {
        [JsonIgnore]
        public double Calories { get; set; }

        [JsonIgnore]
        public double Carbohydrate { get; set; }

        [JsonIgnore]
        public double Fat { get; set; }

        [JsonIgnore]
        public double Protein { get; set; }
    }
}
