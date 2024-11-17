// <copyright file="LicenseViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace VeganLife.ViewModels.ContentViewModels
{
    public partial class LicenseViewModel : BaseViewModel
    {
        [ObservableProperty]
        private IList<LicenseModel> _licenses;
        [ObservableProperty]
        private LicenseModel _selectedItem;

        public LicenseViewModel()
            : base()
        {
            this.Init();
        }

        private void Init()
        {
            this.Licenses = new List<LicenseModel>()
            {
                new LicenseModel("Sentry.Maui", "https://raw.githubusercontent.com/getsentry/sentry-dotnet/main/LICENSE"),
                new LicenseModel("sqlite-net-pcl", "https://raw.githubusercontent.com/praeclarum/sqlite-net/master/LICENSE.txt"),
                new LicenseModel("SQLitePCLRaw.bundle_green", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("SQLitePCLRaw.core", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("SQLitePCLRaw.provider.dynamic_cdecl", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("SQLitePCLRaw.provider.e_sqlite3", "https://raw.githubusercontent.com/ericsink/SQLitePCL.raw/master/LICENSE.TXT"),
                new LicenseModel("CommunityToolkit.Maui", "https://raw.githubusercontent.com/CommunityToolkit/Maui/main/LICENSE"),
                new LicenseModel("CommunityToolkit.Mvvm", "https://raw.githubusercontent.com/CommunityToolkit/dotnet/main/License.md"),
                new LicenseModel("akgul.Maui.DataGrid", "https://raw.githubusercontent.com/akgulebubekir/Maui.DataGrid/main/LICENSE"),
                new LicenseModel("CardsView.Maui", "https://raw.githubusercontent.com/AndreiMisiukevich/CardView.MAUI/main/LICENSE"),
                new LicenseModel("FFImageLoading.Maui", "https://raw.githubusercontent.com/microspaze/FFImageLoading.Maui/main/LICENSE.md"),
                new LicenseModel("FirebaseDatabase.net", "https://raw.githubusercontent.com/step-up-labs/firebase-database-dotnet/master/LICENSE"),
                new LicenseModel("LiveChartsCore.SkiaSharpView.Maui", "https://raw.githubusercontent.com/beto-rodriguez/LiveCharts2/master/LICENSE"),
                new LicenseModel("HtmlAgilityPack", "https://raw.githubusercontent.com/zzzprojects/html-agility-pack/blob/master/LICENSE"),
                new LicenseModel("Mopups", "https://raw.githubusercontent.com/LuckyDucko/Mopups/blob/master/LICENSE"),
                new LicenseModel("Sharpnado.Tabs.Maui", "https://raw.githubusercontent.com/roubachof/Sharpnado.Tabs/blob/main/LICENSE"),
                new LicenseModel("ChatGptNet", "https://raw.githubusercontent.com/marcominerva/ChatGptNet/blob/master/LICENSE"),
                new LicenseModel("SkiaSharp.Extended.UI.Maui", "https://raw.githubusercontent.com/mono/SkiaSharp.Extended/main/LICENSE"),
                new LicenseModel("Sharpnado.MaterialFrame.Maui", "https://raw.githubusercontent.com/roubachof/Sharpnado.MaterialFrame/blob/master/LICENSE"),
                new LicenseModel("Utf8Json", "https://raw.githubusercontent.com/neuecc/Utf8Json/master/LICENSE"),
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
