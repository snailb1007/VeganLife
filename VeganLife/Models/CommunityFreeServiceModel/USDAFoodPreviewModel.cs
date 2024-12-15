using Newtonsoft.Json;
using SQLite;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.Models.CommunityFreeServiceModel
{
    public partial class USDAFoodPreviewModel
    {
        [PrimaryKey]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("isPlantOrigin")]
        public bool IsPlantOrigin { get; set; }
    }

    public partial class USDAFoodPreviewModel : ObservableObject
    {
        [ObservableProperty, JsonIgnore]
        private int _countCorrectWordOnSearch;

        [Ignore]
        public bool IsUSDAFood => !string.IsNullOrEmpty(Id) && !Id.Contains(ConstantHelper.TAG);

        [Ignore]
        public bool IsShowingEdit { get; set; }

        [Ignore]
        public int Amount { get; set; }
    }
}
