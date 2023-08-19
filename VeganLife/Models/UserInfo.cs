using SQLite;
using VeganLife.Helpers.AppSetting;

namespace VeganLife.Models
{
    public partial class UserInfo
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMale { get; set; }
        public float Weight { get; set; }
        public short Height { get; set; }
        public string FoodsRead { get; set; }
        public string VitaminRead { get; set; }
        public uint TotalRead { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public partial class UserInfo
    {
        public byte Age => (byte)(DateTime.Today.Subtract(DateOfBirth).TotalDays / ConstantHelper.AverageDaysInYear);
    }
}
