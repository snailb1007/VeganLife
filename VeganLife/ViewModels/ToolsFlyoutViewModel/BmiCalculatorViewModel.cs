using VeganLife.Helpers;
using VeganLife.Services.CommunityFreeService;

namespace VeganLife.ViewModels.ToolsFlyoutViewModel
{
    public partial class BmiCalculatorViewModel : BaseViewModel
    {
        public BmiCalculatorViewModel()
           : base()
        {
        }

        public override async Task<Task> ViewAppearingVM()
        {
            var x = await ServicesHelper.GetService<USDAApiService>().GetFoodDetailsByIdAsync(2038883);
            Console.WriteLine("==> thien " + x);
            return base.ViewAppearingVM();
        }
    }
}
