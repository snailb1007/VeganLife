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

        public float BMIResult { get; set; }

        public uint TotalFoodDetailRead { get; set; }

        public uint TotalVitaminRead { get; set; }

        public uint TotalDiscoveryRead { get; set; }

        public float BMRResult { get; set; }

        public float TDEEResult { get; set; }

        public string ActivityLevel { get; set; }
    }

    public partial class UserInfo
    {
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - this.DateOfBirth.Year;

                if (DateOfBirth.Date > today.AddYears(-age))
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
    }
}
