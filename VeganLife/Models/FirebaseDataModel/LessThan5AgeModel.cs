using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class LessThan5Age
    {
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("documents")]
        public string Documents { get; set; }
    }
}
