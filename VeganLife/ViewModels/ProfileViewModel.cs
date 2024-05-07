// <copyright file="ProfileViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        const byte totalNumberSpec = 4;

        [ObservableProperty]
        UserInfo myInfo;

        [ObservableProperty]
        byte numberInfoMiss;

        [ObservableProperty]
        int degreePerfection;

        private readonly IUserDataService userDataService;
        public ProfileViewModel()
            : base()
        {
            MyInfo = new UserInfo();
            userDataService = ServicesHelper.GetService<IUserDataService>();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            IsLoading = true;
            await this.userDataService.Refresh();
            MyInfo = (userDataService as UserDataService)?.UserInfo!;
            NumberInfoMiss = totalNumberSpec;

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

                DegreePerfection = (totalNumberSpec - NumberInfoMiss) / totalNumberSpec * 100;
            }

            IsLoading = false;
            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task Close()
        {
            if (CloseCommand.IsRunning)
            {
                return;
            }

            await this.navigationService.PopAsync();
        }

        // partial void OnNumberInfoMissChanged(byte value)
        // {
        //    DegreePerfection = (byte)(value * 25);
        // }
    }
}
