using VeganLife.Data.LocalData;
using VeganLife.Helpers;

namespace VeganLife.Services.UserServices
{
    public class UserDataService : IUserDataService
    {
        private readonly UserInfoDataStoreServie _userInfoDataStoreServie;

        private UserInfo _userInfo;
        private bool _hasOldData;

        public UserDataService()
        {
            this._userInfo = new UserInfo();
            _userInfoDataStoreServie = ServicesHelper.GetService<UserInfoDataStoreServie>();
            _ = InitAsync();
        }

        public async Task InitAsync()
        {
            UserInfo temp = new();
            var currentDeviceID = ServicesHelper.GetService<IDeviceService>().GetDeviceId();
            temp.Id = currentDeviceID;

            await Refresh();

            if (currentDeviceID != _userInfo?.Id
                || string.IsNullOrEmpty(_userInfo?.Name))
            {
                var userHasName = (await _userInfoDataStoreServie.GetItemsAsync())?
                    .FirstOrDefault(u => !string.IsNullOrEmpty(u.Name))!;
                if (userHasName != null)
                {
                    temp.Name = userHasName.Name;
                    temp.Weight = userHasName.Weight;
                    temp.Height = userHasName.Height;
                    temp.DateOfBirth = userHasName.DateOfBirth;
                    temp.IsMale = userHasName.IsMale;
                }

                await _userInfoDataStoreServie.DeleteAllItems();
                await _userInfoDataStoreServie.AddOrUpdateItemAsync(temp);
                _userInfo = temp;
            }
        }

        public async Task Refresh()
        {
            var localUser = await _userInfoDataStoreServie.GetFirstOrDefaultItem();
            if (localUser != null)
            {
                _userInfo = localUser;
                _hasOldData = true;
            }
        }

        public async Task SaveData(UserInfo data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return;
            }

            var bmi = BMICalculateHelper.Calculate(data.Weight, data.Height / 100f);
            this._userInfo.Name = data.Name;
            this._userInfo.DateOfBirth = data.DateOfBirth;
            this._userInfo.Weight = data.Weight;
            this._userInfo.Height = data.Height;
            this._userInfo.IsMale = data.IsMale;
            this._userInfo.BMIResult = (float)bmi;

            await this.SaveData();
        }

        public async Task SaveData()
        {
            await _userInfoDataStoreServie.AddOrUpdateItemAsync(this._userInfo, true);
        }

        UserInfo IUserDataService.GetUserInfo()
        {
            return this._userInfo;
        }
    }
}
