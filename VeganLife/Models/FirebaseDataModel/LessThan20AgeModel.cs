// <copyright file="LessThan20AgeModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: Firebase
using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record LessThan20AgeModel(
        [property: JsonProperty("note")] string Note,
        [property: JsonProperty("documents")] string Documents,
        [property: JsonProperty("chart_link")] ChartLinkModel ChartLink);
}
