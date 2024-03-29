// Ignore Spelling: Firebase
// Ignore Spelling: Bmi

using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record BmiDocumentsModel
        ([property: JsonProperty("who")] string WHO, [property: JsonProperty("wikiVN")] string WikiVN);
}
