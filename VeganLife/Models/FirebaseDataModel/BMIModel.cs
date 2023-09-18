using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class BMIModel
    {
        [JsonProperty("documents")]
        public BMIDocumentsModel Documents { get; set; }
        [JsonProperty("lessThan5Age")]
        public LessThan5Age LessThan5Age { get; set; }
        [JsonProperty("lessThan20Age")]
        public LessThan20AgeModel LessThan20Age { get; set; }
        [JsonProperty("adults")]
        public AdultModel Adult { get; set; }
    }
}
