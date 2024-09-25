
using VeganLife.Helpers;
using VeganLife.Helpers.Extensions;
using VeganLife.Models.CommunityFreeServiceModel;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels
{
    public partial class USDAFoodListPageVM : BaseViewModel
    {
        [ObservableProperty]
        private ObservableRangeCollection<FoodDetailListRequest> uSDAFoods = new();

        public override async Task ViewAppearingVM()
        {
            if (this.USDAFoods.Any())
            {
                return;
            }

            var request = new FoodsListRequestModel
            {
                DataType = new List<string> { "Survey (FNDDS)" },
                PageSize = 10,
                PageNumber = 1,
                SortBy = "dataType.keyword",
                SortOrder = "desc",
            };
            var data = await ServicesHelper.GetService<USDAApiService>().GetFoodsListAsync(request);
            this.USDAFoods.Replace(data);
            await base.ViewAppearingVM();
        }
    }
}
