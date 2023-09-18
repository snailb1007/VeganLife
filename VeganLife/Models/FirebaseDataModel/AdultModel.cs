using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class AdultModel
    {
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("documents")]
        public string Documents { get; set; }
    }
}
