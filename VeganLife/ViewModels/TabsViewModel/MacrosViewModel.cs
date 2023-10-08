using System.Text.RegularExpressions;
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
                    await this.dataService.GetFoodsUSDA()
                   .ContinueWith(t =>
                   {
                       this.allUSDAFoodPreview = new List<USDAFoodPreviewModel>(t.Result);
                   });
                }

                UsdaFoodPreviews = new ObservableCollection<USDAFoodPreviewModel>(allUSDAFoodPreview);
            }

            return base.ViewAppearingVM();
        }

        [RelayCommand]
        private async Task ItemSelectedChanged()
        {
            if (this.UsdaFoodPreviewCurrent != null)
            {
                await navigationService.NavigateToPage<UsdaFoodFactDetailPage>(this.UsdaFoodPreviewCurrent);
                this.UsdaFoodPreviewCurrent = null;
            }
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
                int count = (from word in words
                             where normalName.Contains(word)
                             select word).Count();
                item.CountCorrectWordOnSearch = count;
            }

            return allUSDAFoodPreview
                .Where(w => w.CountCorrectWordOnSearch == words.Length)
                .OrderByDescending(i => i.CountCorrectWordOnSearch);
        }
    }
}
