using Android.Hardware.Lights;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;

namespace VeganLife.Services.UserServices
{
    public class UserDataService : IUserDataService
    {
        private readonly UserInfoDataStoreServie _userInfoDataStoreServie;

        public UserInfo UserInfo { get; set; }

        private bool _hasOldData;

        public UserDataService()
        {
            this.UserInfo = new UserInfo();
            _userInfoDataStoreServie = ServicesHelper.GetService<UserInfoDataStoreServie>();
        }

        public async Task Init()
        {
            UserInfo temp = new();
            var currentDeviceID = ServicesHelper.GetService<IDeviceService>().GetDeviceId();
            temp.Id = currentDeviceID;

            UserInfo = await _userInfoDataStoreServie.GetFirstOrDefaultItem();
            if (currentDeviceID != UserInfo?.Id)
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
                UserInfo = temp;
            }

            //this._hasOldData = UserInfo == null || !string.IsNullOrEmpty(UserInfo.Name);
            //if (!_hasOldData)
            //{
            //    _hasOldData = true;
            //    UserInfo = new UserInfo();
            //    UserInfo.Id = ServicesHelper.GetService<IDeviceService>().GetDeviceId();
            //    UserInfo.TotalFoodDetailRead = 0;
            //    UserInfo.TotalVitaminRead = 0;
            //    UserInfo.TotalDiscoveryRead = 0;
            //    await _userInfoDataStoreServie.AddOrUpdateItemAsync(this.UserInfo);
            //}
        }

        public async Task Refresh()
        {
            this.UserInfo = await _userInfoDataStoreServie.GetFirstOrDefaultItem();
        }

        //public short GetUserAge()
        //{
        //    return this.UserInfo.Age;
        //}

        public async Task<string> GetUserNameAsync()
        {
            if (!_hasOldData)
            {
                await Init();
            }

            return this.UserInfo.Name ?? string.Empty;
        }

        public async Task SaveData(UserInfo data)
        {
            if (string.IsNullOrEmpty(data.Name))
            {
                return;
            }

            var bmi = BMICalculateHelper.Calculate(data.Weight, data.Height / 100f);
            this.UserInfo.Name = data.Name;
            this.UserInfo.DateOfBirth = data.DateOfBirth;
            this.UserInfo.Weight = data.Weight;
            this.UserInfo.Height = data.Height;
            this.UserInfo.IsMale = data.IsMale;
            this.UserInfo.BMIResult = (float)bmi;

            await this.SaveData();
        }

        public async Task SaveData()
        {
            await _userInfoDataStoreServie.AddOrUpdateItemAsync(this.UserInfo, true);
        }

        //public DateTime GetDateOfBirth()
        //{
        //    return this.UserInfo.DateOfBirth;
        //}

        //public float GetWeight()
        //{
        //    return this.UserInfo.Weight;
        //}

        //public short GetHeight()
        //{
        //    return this.UserInfo.Height;
        //}
    }
}
