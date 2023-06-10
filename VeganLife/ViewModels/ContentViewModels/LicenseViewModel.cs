namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class LicenseViewModel : BaseViewModel
    {
        [ObservableProperty]
        IList<string> _licenses;
        public LicenseViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            Init();
        }

        void Init()
        {
            Licenses = new List<string>()
            {
                "CommunityToolkit.Maui",
                "CommunityToolkit.Mvvm",
                "FirebaseDatabase.net",
                "FFImageLoadingCompat.Maui",
                "LiveChartsCore.SkiaSharpView.Maui",
                "PureWeen.Maui.FixesAndWorkarounds",
                "sqlite-net-pcl",
                "SQLitePCLRaw.bundle_green",
                "SQLitePCLRaw.core",
                "SQLitePCLRaw.provider.dynamic_cdecl",
                "SQLitePCLRaw.provider.e_sqlite3"
            };
        }
    }
}
