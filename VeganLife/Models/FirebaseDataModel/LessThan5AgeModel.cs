// Ignore Spelling: Firebase

using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record LessThan5Age(
        [property: JsonProperty("note")] string Note,
        [property: JsonProperty("documents")] string Documents);
}
