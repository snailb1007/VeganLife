// <copyright file="AdultModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

// Ignore Spelling: Firebase
using Newtonsoft.Json;

namespace VeganLife.Models.FirebaseDataModel
{
    public record AdultModel(
        [property: JsonProperty("note")] string Note,
        [property: JsonProperty("documents")] string Documents);
}
