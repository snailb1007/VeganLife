// <copyright file="IUserDataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services.UserServices
{
    public interface IUserDataService
    {
        UserInfo GetUserInfo();

        Task InitAsync();

        Task Refresh();

        Task SaveData(UserInfo data);

        Task SaveData();
    }
}
