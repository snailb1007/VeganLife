using System.Text.RegularExpressions;
using VeganLife.Data.LocalData;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Views.PortionTab;

namespace VeganLife.ViewModels.TabsViewModel
{
    public partial class MacrosViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<USDAFoodPreviewModel> usdaFoodPreviews;
        [ObservableProperty]
        private string textSearch;
        [ObservableProperty]
        private USDAFoodPreviewModel usdaFoodPreviewCurrent;

        private List<USDAFoodPreviewModel> allUSDAFoodPreview;
        //private IEnumerable<USDAFoodPreviewModel> passedFoodFilter;
        public MacrosViewModel()
            : base()
        {
        }

        public override async Task<Task> ViewAppearingVM()
        {
            if (!UsdaFoodPreviews?.Any() ?? true)
            {
                if (!allUSDAFoodPreview?.Any() ?? true)
                {
                    var foodData = await this.dataService.GetFoodsUSDA();
                    this.allUSDAFoodPreview = new List<USDAFoodPreviewModel>(foodData);
                }

                App.Current?.MainPage?.Dispatcher?
                    .Dispatch(() => UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>
                    (allUSDAFoodPreview ?? new List<USDAFoodPreviewModel>()));
            }

            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task ItemSelectedChanged(USDAFoodPreviewModel data)
        {
            if (ItemSelectedChangedCommand.IsRunning || IsLoading || data is null)
                return;
            IsLoading = true;
            await navigationService.NavigateToPage<UsdaFoodFactDetailPage>(data);
            IsLoading = false;
        }

        [RelayCommand]
        private void EnsureSearch()
        {
            var x = this.FilterKeySearch(allUSDAFoodPreview, this.TextSearch);
            this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(x);
        }

        partial void OnTextSearchChanged(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
            {
                this.UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(this.allUSDAFoodPreview);
            }
        }

        private IEnumerable<USDAFoodPreviewModel> FilterKeySearch(List<USDAFoodPreviewModel> foods, string key)
        {
            string[] words = Regex.Replace(key, @"\s+", " ").Split(' ');
            foreach (var item in foods)
            {
                var normalName = item.Name.ConvertStringToUnSigned() ?? string.Empty;
                //int count = (from word in words
                //             where normalName.Contains(word)
                //             select word).Count();
                int count = words.Count(word => normalName.Contains(word, StringComparison.InvariantCultureIgnoreCase));
                item.CountCorrectWordOnSearch = count;
            }

            return allUSDAFoodPreview
                .Where(w => w.CountCorrectWordOnSearch == words.Length)
                .OrderByDescending(i => i.CountCorrectWordOnSearch);
        }

        [RelayCommand]
        private async Task OnSupportRequest()
        {
            var templateTask = ResourceReader.ReadTextFileAsync("VeganLife.Resources.Raw.mail_template.txt");
            var userTask = ServicesHelper.GetService<UserInfoDataStoreServie>().GetItemAsync();
            await Task.WhenAll(templateTask, userTask);
            var content = templateTask.Result.Replace("@@@username@@@", userTask?.Result?.Name);
            content = content.Replace("@@@content@@@", TextSearch);
            await ServicesHelper.GetService<IDeviceService>().SendEmailAsync("Support Request", content, new List<string> { "cskhveganlife@gmail.com" });
        }
    }
}
