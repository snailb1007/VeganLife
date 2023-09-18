using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class ChartLinkModel
    {
        [JsonProperty("boy")]
        public string Boy { get; set; }
        [JsonProperty("girl")]
        public string Girl { get; set; }
    }
}
