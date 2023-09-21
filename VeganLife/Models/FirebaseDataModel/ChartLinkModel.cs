// Ignore Spelling: Firebase

using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record ChartLinkModel(
        [property: JsonProperty("boy")] string Boy,
        [property: JsonProperty("girl")] string Girl);
}
