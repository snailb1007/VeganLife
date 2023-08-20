using VeganLife.Data.LocalData;
using VeganLife.Helpers;

namespace VeganLife.Services.UserServices
{
    public class UserDataService : IUserDataService
    {
        public UserInfo UserInfo { get; set; }
        private bool hasOldData;
        public UserDataService()
        {
            this.UserInfo = new UserInfo();
        }

        private async Task Init()
        {
            UserInfo = await ServicesHelper.GetService<UserInfoDataStoreServie>().GetFirstOrDefaultItem();
            this.hasOldData = UserInfo != null;
            if (!hasOldData)
            {
                hasOldData = true;
                UserInfo = new UserInfo();
                UserInfo.Id = ServicesHelper.GetService<IDeviceService>().GetDeviceId();
                await ServicesHelper.GetService<UserInfoDataStoreServie>().AddOrUpdateItemAsync(this.UserInfo);
            }
        }

        public short GetUserAge()
        {
            return this.UserInfo.Age;
        }

        public async Task<string> GetUserNameAsync()
        {
            if (!hasOldData)
            {
                await Init();
            }

            return this.UserInfo.Name ?? string.Empty;
        }

        public async Task SaveData(DateTime dateOfBirth, string name = null, bool isMale = false, short height = 0, float weight = 0)
        {
            if (!string.IsNullOrEmpty(name))
                this.UserInfo.Name = name;
            this.UserInfo.IsMale = isMale;
            this.UserInfo.Height = height;
            this.UserInfo.Weight = weight;
            this.UserInfo.DateOfBirth = dateOfBirth;
            await ServicesHelper.GetService<UserInfoDataStoreServie>().AddOrUpdateItemAsync(this.UserInfo, true);
        }

        public DateTime GetDateOfBirth()
        {
            return this.UserInfo.DateOfBirth;
        }

        public float GetWeight()
        {
            return this.UserInfo.Weight;
        }

        public short GetHeight()
        {
            return this.UserInfo.Height;
        }
    }
}
