using SQLite;

namespace VeganLife.Models
{
    public partial class GoogleAdValidatorModel
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public DateTime LastTimeRewardOpen { get; set; }

        public DateTime LastTimeBannerOpen { get; set; }

        public byte RewardAdTimesLimit { get; set; }
    }

    public partial class GoogleAdValidatorModel
    {
        public bool IsRewardAdAvailable => this.RewardAdTimesLimit < 3;
    }
}