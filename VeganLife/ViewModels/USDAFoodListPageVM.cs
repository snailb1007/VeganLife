using Microsoft.Maui.Adapters;
using VeganLife.Helpers;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels
{
    public partial class USDAFoodListPageVM : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollectionAdapter<FoodDetailListRequest> _uSDAFoods;

        public override async Task ViewAppearingVM()
        {
            await base.ViewAppearingVM();
            if (this.USDAFoods != null)
            {
                return;
            }

            var request = new FoodsListRequestModel
            {
                DataType = new List<string> { "Survey (FNDDS)" },
                PageSize = 200,
                PageNumber = 1,
                SortBy = "dataType.keyword",
                SortOrder = "desc",
            };
            var data = await ServicesHelper.GetService<USDAApiService>().GetFoodsListAsync(request);
            if (data?.Any() == false)
            {
                return;
            }

            if (data != null)
            {
                this.USDAFoods = new ObservableCollectionAdapter<FoodDetailListRequest>(
                    new ObservableCollection<FoodDetailListRequest>(data));
            }
        }
    }
}
