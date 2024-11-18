using Newtonsoft.Json;
using VeganLife.Helpers;

namespace VeganLife.Models.BaseModel
{
    public abstract class BaseNutrientModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public string VietnameseName => StringProcessHelper.GetNameContainVietnameseTranslations(enName: this.Name ?? string.Empty);
    }
}
