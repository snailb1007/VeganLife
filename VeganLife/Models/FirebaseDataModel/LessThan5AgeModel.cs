using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class LessThan5Age
    {
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
