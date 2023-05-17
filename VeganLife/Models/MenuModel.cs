using Newtonsoft.Json;

namespace VeganLife.Models
{
    public partial class MenuModel : ObservableObject
    {
        [JsonProperty("image_landscape")]
        public string ImgSource { get; set; }
        [JsonIgnore]
        public string Title { get; set; }
    }
}
