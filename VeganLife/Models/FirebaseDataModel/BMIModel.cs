// <copyright file="BMIModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: Bmi, Firebase
using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record BmiModel(
        [property: JsonProperty("documents")] BmiDocumentsModel Documents,
        [property: JsonProperty("lessThan5Age")] LessThan5AgeModel LessThan5Age,
        [property: JsonProperty("lessThan20Age")] LessThan20AgeModel LessThan20Age,
        [property: JsonProperty("adults")] AdultModel Adult);
}
