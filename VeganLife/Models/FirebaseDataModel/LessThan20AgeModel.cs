using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public class LessThan20AgeModel
    {
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("documents")]
        public string Documents { get; set; }
        [JsonProperty("chart_link")]
        public ChartLinkModel ChartLink { get; set; }
    }


}
