using Newtonsoft.Json;
using PropertyChanged;
using SQLite;

namespace VeganLife.Models.FoodModel
{
    public partial class FoodPreviewModel
    {
        [JsonIgnore, PrimaryKey]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public partial class FoodPreviewModel
    {
        public bool IsBookmarked { get; set; }
        public bool IsRead { get; set; }
    }
}
