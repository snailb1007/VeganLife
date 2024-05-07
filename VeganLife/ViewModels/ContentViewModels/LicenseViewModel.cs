// <copyright file="LicenseViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class LicenseViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IList<LicenseModel>? licenses;
        [ObservableProperty]
        private LicenseModel? selectedItem;

        public LicenseViewModel()
            : base()
        {
            this.Init();
        }

        private void Init()
        {
            this.Licenses = new List<LicenseModel>()
            {
                new LicenseModel("CommunityToolkit.Maui", "https://raw.githubusercontent.com/CommunityToolkit/Maui/main/LICENSE"),
                new LicenseModel("CommunityToolkit.Mvvm", "https://raw.githubusercontent.com/CommunityToolkit/dotnet/main/License.md"),
                new LicenseModel("FirebaseDatabase.net", "https://raw.githubusercontent.com/step-up-labs/firebase-database-dotnet/master/LICENSE"),
                new LicenseModel("FFImageLoadingCompat.Maui", "https://raw.githubusercontent.com/Redth/FFImageLoading.Compat/main/LICENSE.md"),
                new LicenseModel("LiveChartsCore.SkiaSharpView.Maui", "https://raw.githubusercontent.com/beto-rodriguez/LiveCharts2/master/LICENSE"),
                new LicenseModel("PureWeen.Maui.FixesAndWorkarounds", "https://raw.githubusercontent.com/PureWeen/ShanedlerSamples/main/LICENSE"),
                new LicenseModel("sqlite-net-pcl", "https://raw.githubusercontent.com/praeclarum/sqlite-net/master/LICENSE.txt"),
                new LicenseModel("SQLitePCLRaw.bundle_green", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("SQLitePCLRaw.core", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("SQLitePCLRaw.provider.dynamic_cdecl", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("SQLitePCLRaw.provider.e_sqlite3", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
            };
        }

        [RelayCommand]
        private async Task OpenLicenseDetail()
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            await Browser.Default.OpenAsync(this.SelectedItem.LicenseLink, BrowserLaunchMode.SystemPreferred);
            this.SelectedItem = null;
        }
    }
}
