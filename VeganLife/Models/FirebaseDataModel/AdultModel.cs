using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class AdultModel
    {
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
