using Plugin.MauiMTAdmob;
using System.Timers;

namespace VeganLife.Services
{
    internal class GoogleAdService
    {
        private static GoogleAdService _instance;

        //private int _bannerAdCount;
        private int _rewardAdCount;
        private System.Timers.Timer _resetTimer;

        public static GoogleAdService Instance => _instance ?? (_instance = new GoogleAdService());

        private GoogleAdService()
        {
            //_bannerAdCount = 0;
            _rewardAdCount = 0;

            _resetTimer = new System.Timers.Timer(60000); // 1 minute
            _resetTimer.Elapsed += ResetAdCounts!;
            _resetTimer.Start();
        }

        private void ResetAdCounts(object sender, ElapsedEventArgs e)
        {
            //_bannerAdCount = 0;
            _rewardAdCount = 0;
        }

        //public void ShowBannerAd()
        //{
        //    if (_bannerAdCount < 3)
        //    {
        //        //CrossMauiMTAdmob.Current.ShowBanner();
        //        _bannerAdCount++;
        //    }
        //}

        public void ShowRewardAd()
        {
            if (_rewardAdCount < 3)
            {
                //CrossMauiMTAdmob.Current.ShowRewardVideo();
                _rewardAdCount++;
            }
        }
    }
}