// <copyright file="IUserDataService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.Services.UserServices
{
    public interface IUserDataService
    {
        public UserInfo UserInfo { get; set; }

        Task Init();

        Task<string> GetUserNameAsync();

        Task Refresh();

        Task SaveData(UserInfo data);

        Task SaveData();
    }
}
