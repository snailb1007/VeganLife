// <copyright file="LicenseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models
{
    public class LicenseModel
    {
        public string Name { get; private set; }

        public string LicenseLink { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseModel"/> class.
        /// </summary>
        /// <param name="name">name of nuget pack.</param>
        /// <param name="licenseLink"> link's license of nuget.</param>
        public LicenseModel(string name, string licenseLink)
        {
            this.Name = name;
            this.LicenseLink = licenseLink;
        }
    }
}
