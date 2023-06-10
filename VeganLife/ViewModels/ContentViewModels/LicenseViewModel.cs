namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class LicenseViewModel : BaseViewModel
    {
        [ObservableProperty]
        IList<LicenseModel> _licenses;
        [ObservableProperty]
        LicenseModel _selectedItem;
        public LicenseViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService, dataService)
        {
            Init();
        }

        void Init()
        {
            Licenses = new List<LicenseModel>()
            {
                new LicenseModel("CommunityToolkit.Maui", ""),
                new LicenseModel("CommunityToolkit.Mvvm", ""),
                new LicenseModel("FirebaseDatabase.net", ""),
                new LicenseModel("FFImageLoadingCompat.Maui", ""),
                new LicenseModel("LiveChartsCore.SkiaSharpView.Maui", ""),
                new LicenseModel("PureWeen.Maui.FixesAndWorkarounds", ""),
                new LicenseModel("sqlite-net-pcl", ""),
                new LicenseModel("SQLitePCLRaw.bundle_green", ""),
                new LicenseModel("SQLitePCLRaw.core", ""),
                new LicenseModel("SQLitePCLRaw.provider.dynamic_cdecl", ""),
                new LicenseModel("SQLitePCLRaw.provider.e_sqlite3", "")
            };
        }

        [RelayCommand]
        async Task OpenLicenseDetail()
        {
            await Task.Delay(1);
            SelectedItem = null;
        }
    }

    public class LicenseModel
    {
        public string Name { get; private set; }
        public string LicenseLink { get; private set; }

        public LicenseModel(string name, string licenseLink)
        {
            Name = name;
            LicenseLink = licenseLink;
        }
    }
}
