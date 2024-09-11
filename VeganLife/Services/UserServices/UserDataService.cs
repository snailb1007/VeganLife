using AsyncAwaitBestPractices;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;

namespace VeganLife.Services.UserServices
{
    public class UserDataService : IUserDataService
    {
        private readonly UserInfoDataStoreServie _userInfoDataStoreServie;

        private UserInfo _userInfo;
        private bool _hasOldData;
        private Task _currentInitTask;

        public UserDataService()
        {
            this._userInfo = new UserInfo();
            _userInfoDataStoreServie = ServicesHelper.GetService<UserInfoDataStoreServie>();
            _currentInitTask = InitAsync();
            _currentInitTask.SafeFireAndForget();
        }

        public async Task InitAsync()
        {
            if (this._currentInitTask != null && !this._currentInitTask.IsCompleted)
            {
                await _currentInitTask;
            }

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

            bool isDataChanged = false;

            if (this._userInfo.Name != data.Name)
            {
                this._userInfo.Name = data.Name;
            }

            if (this._userInfo.Weight != data.Weight)
            {
                this._userInfo.Weight = data.Weight;
                isDataChanged = true;
            }

            if (this._userInfo.Height != data.Height)
            {
                this._userInfo.Height = data.Height;
                isDataChanged = true;
            }

            if (this._userInfo.DateOfBirth != data.DateOfBirth)
            {
                this._userInfo.DateOfBirth = data.DateOfBirth;
                isDataChanged = true;
            }

            if (this._userInfo.IsMale != data.IsMale)
            {
                this._userInfo.IsMale = data.IsMale;
                isDataChanged = true;
            }

            var bmi = BMICalculateHelper.Calculate(data.Weight, data.Height / 100f);
            if (this._userInfo.BMIResult != (float)bmi)
            {
                this._userInfo.BMIResult = (float)bmi;
                isDataChanged = true;
            }

            if (this._userInfo.ActivityLevelData != data.ActivityLevelData)
            {
                this._userInfo.ActivityLevelData = data.ActivityLevelData;
                isDataChanged = true;
            }

            this._userInfo.BMRResult = TDEEHelper.CalculateBMR(data.Weight, data.Height, data.Age, data.IsMale);
            if (this._userInfo.TDEEResult != data.TDEEResult)
            {
                this._userInfo.TDEEResult = data.TDEEResult;
                isDataChanged = true;
            }

            Debug.WriteLine($"SaveData: {isDataChanged}");
            Debug.WriteLine($"Name: {this._userInfo.Name}");
            Debug.WriteLine($"Weight: {this._userInfo.Weight}");
            Debug.WriteLine($"Height: {this._userInfo.Height}");
            Debug.WriteLine($"DateOfBirth: {this._userInfo.DateOfBirth}");
            Debug.WriteLine($"IsMale: {this._userInfo.IsMale}");
            Debug.WriteLine($"BMIResult: {this._userInfo.BMIResult}");
            Debug.WriteLine($"ActivityLevelData: {this._userInfo.ActivityLevelData}");
            Debug.WriteLine($"BMRResult: {this._userInfo.BMRResult}");
            Debug.WriteLine($"TDEEResult: {this._userInfo.TDEEResult}");

            await this.SaveData();
        }

        public Task SaveData()
        {
            return _userInfoDataStoreServie.AddOrUpdateItemAsync(this._userInfo, true);
        }

        UserInfo IUserDataService.GetUserInfo()
        {
            return this._userInfo;
        }
    }
}
