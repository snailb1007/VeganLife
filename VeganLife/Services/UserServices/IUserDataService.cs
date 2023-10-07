namespace VeganLife.Services.UserServices
{
    public interface IUserDataService
    {
        Task<string> GetUserNameAsync();
        Task Refresh();
        short GetUserAge();
        DateTime GetDateOfBirth();
        float GetWeight();
        short GetHeight();
        Task SaveData(DateTime dateOfBirth, string name = null, bool isMale = false, short height = 0, float weight = 0);
        Task SaveData();
    }
}
