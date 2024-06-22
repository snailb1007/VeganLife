// <copyright file="ProfileViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using VeganLife.Helpers;
using VeganLife.Services.UserServices;

namespace VeganLife.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private const byte totalNumberSpec = 4;

        private readonly IUserDataService _userDataService;

        [ObservableProperty]
        private UserInfo myInfo;

        [ObservableProperty]
        private byte numberInfoMiss;

        [ObservableProperty]
        private int degreePerfection;

        public ProfileViewModel()
            : base()
        {
            MyInfo = new UserInfo();
            _userDataService = ServicesHelper.GetService<IUserDataService>();
        }

        public override async Task ViewAppearingVM()
        {
            using (await this.loadingService.Show())
            {
                await this._userDataService.Refresh();
                MyInfo = (_userDataService as UserDataService)?.UserInfo!;
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

                await base.ViewAppearingVM();
            }
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