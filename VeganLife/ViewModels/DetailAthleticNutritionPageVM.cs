namespace VeganLife.ViewModels
{
    public partial class DetailAthleticNutritionPageVM : BaseViewModel
    {
        [ObservableProperty]
        private AthleticNutritionModel _athleticNutrition;

        public override Task OnNavigatingTo(object parameter)
        {
            if (parameter is AthleticNutritionModel model)
            {
                this.AthleticNutrition = model;
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}