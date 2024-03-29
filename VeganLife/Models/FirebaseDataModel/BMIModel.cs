// Ignore Spelling: Firebase
// Ignore Spelling: Bmi

using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record BmiModel(
        [property: JsonProperty("documents")] BmiDocumentsModel Documents,
        [property: JsonProperty("lessThan5Age")] LessThan5Age LessThan5Age,
        [property: JsonProperty("lessThan20Age")] LessThan20AgeModel LessThan20Age,
        [property: JsonProperty("adults")] AdultModel Adult);
}
