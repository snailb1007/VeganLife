using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class LessThan20AgeModel
    {
        [JsonProperty("note")]
        public string Note { get; set; }
    }
}
