using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganLife.ViewModels
{
    public partial class DetailAthleticNutritionPageVM : BaseViewModel
    {
        [ObservableProperty]
        private AthleticNutritionModel athleticNutrition;

        public DetailAthleticNutritionPageVM()
            : base()
        {
        }

        public override Task OnNavigatingTo(object? parameter)
        {
            if (parameter is AthleticNutritionModel model)
            {
                this.AthleticNutrition = model;
            }

            return base.OnNavigatingTo(parameter);
        }
    }
}