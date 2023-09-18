using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class BMIDocumentsModel
    {
        [JsonProperty("who")]
        public string WHO { get; set; }
        [JsonProperty("wikiVN")]
        public string WikiVN { get; set; }
    }
}
