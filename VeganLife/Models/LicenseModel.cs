// <copyright file="LicenseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Models
{
    public class LicenseModel
    {
        public string Name { get; private set; }

        public string LicenseLink { get; private set; }

        public LicenseModel(string name, string licenseLink)
        {
            this.Name = name;
            this.LicenseLink = licenseLink;
        }
    }
}
