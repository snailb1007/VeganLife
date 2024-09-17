using SQLite;
using static VeganLife.Helpers.AppSetting.ConstantHelper.CalculateHelper;

namespace VeganLife.Models
{
    public partial class UserInfo : INotifyPropertyChanged
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsMale { get; set; }

        public float Weight { get; set; }

        public short Height { get; set; }

        public string FoodsRead { get; set; }

        public string VitaminRead { get; set; }

        public uint TotalRead { get; set; } = 0;

        public DateTime? DateOfBirth { get; set; } = null;

        public float BMIResult { get; set; } = -1;

        public uint TotalFoodDetailRead { get; set; } = 0;

        public uint TotalVitaminRead { get; set; } = 0;

        public uint TotalDiscoveryRead { get; set; } = 0;

        public float BMRResult { get; set; } = -1;

        public float TDEEResult { get; set; } = -1;

        public string ActivityLevelData { get; set; }
    }

    public partial class UserInfo
    {
        public int Age
        {
            get
            {
                if (this.DateOfBirth == null || !this.DateOfBirth.HasValue)
                    return -1;
                var today = DateTime.Today;
                var age = today.Year - this.DateOfBirth.Value.Year;

                if (DateOfBirth.Value.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        public string Image => this.IsMale ? "profile_boy" : "profile_girl_strong";

        public bool IsEnoughNecessaryData => !string.IsNullOrEmpty(this.Name)
            && this.Weight != 0
            && this.Height != 0
            && this.DateOfBirth != null
            && this.BMIResult > 0;

        public ActivityLevel NormalFormatActivityLv
        {
            get
            {
                var dataToCheck = ActivityLevelData;
                if (string.IsNullOrEmpty(dataToCheck))
                {
                    dataToCheck = ActivityLevel.Sedentary.ToString();
                }

                return (ActivityLevel)Enum.Parse(typeof(ActivityLevel), dataToCheck);
            }
        }
    }
}
