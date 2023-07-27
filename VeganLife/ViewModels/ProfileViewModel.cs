// <copyright file="ProfileViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Data.LocalData;
using VeganLife.Helpers;

namespace VeganLife.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        [ObservableProperty]
        UserInfo myInfo;

        [ObservableProperty]
        byte numberInfoMiss;

        public ProfileViewModel()
            : base()
        {
            MyInfo = new UserInfo();
        }

        public override async Task<Task> ViewAppearingVM()
        {
            MyInfo = (await ServicesHelper.GetService<UserInfoDataStoreServie>().GetItemsAsync())?.FirstOrDefault();
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
