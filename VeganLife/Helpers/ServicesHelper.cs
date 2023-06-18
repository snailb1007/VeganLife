// <copyright file="ServicesHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Helpers
{
    public static class ServicesHelper
    {
        public static T GetService<T>() => MauiApplication.Current.Services.GetService<T>();
    }
}
