// <copyright file="ProfileViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        UserInfo myInfo;

        [ObservableProperty]
        byte numberInfoMiss;

        private readonly IUserDataService userDataService;
        public ProfileViewModel()
            : base()
        {
            MyInfo = new UserInfo();
            userDataService = ServicesHelper.GetService<IUserDataService>();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            await this.userDataService.Refresh();
            MyInfo = (userDataService as UserDataService).UserInfo;
            NumberInfoMiss = 4;

            if (MyInfo != null)
            {
                if (!string.IsNullOrEmpty(MyInfo.Name))
                {
                    NumberInfoMiss--;
                }

                if (MyInfo.Age > 0)
                {
                    NumberInfoMiss--;
                }

                if (MyInfo.Weight > 0)
                {
                    NumberInfoMiss--;
                }

                if (MyInfo.Height > 0)
                {
                    NumberInfoMiss--;
                }
            }

            return base.ViewAppearingVM();
        }

        [RelayCommand]
        async Task Close()
        {
            if (CloseCommand.IsRunning)
            {
                return;
            }

            await this.navigationService.PopAsync();
        }
    }
}
